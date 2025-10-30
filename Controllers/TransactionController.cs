using ASPPorcelette.API.DTOs.Transaction;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        // GET: api/Transaction
        /// <summary>
        /// Récupère la liste de toutes les transactions avec les détails (Compte, Catégorie, Discipline).
        /// </summary>
        /// <returns>Une liste de TransactionDto.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            if (transactions == null || !transactions.Any())
            {
                return NotFound("Aucune transaction trouvée.");
            }

            // Mappe les entités Transaction sur le DTO de lecture avec détails
            var transactionDtos = _mapper.Map<IEnumerable<TransactionDto>>(transactions);
            return Ok(transactionDtos);
        }

        

        // GET: api/Transaction/5
        /// <summary>
        /// Récupère une transaction spécifique par ID avec tous ses détails.
        /// </summary>
        /// <param name="id">L'ID de la transaction.</param>
        /// <returns>Le TransactionDto correspondant.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TransactionDto>> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);

            if (transaction == null)
            {
                return NotFound($"Transaction avec ID {id} non trouvée.");
            }

            return Ok(_mapper.Map<TransactionDto>(transaction));
        }


// GET: api/Transaction/compte/5
/// <summary>
/// Récupère toutes les transactions associées à un compte donné.
/// </summary>
/// <param name="compteId">L'ID du compte.</param>
/// <returns>Une liste de TransactionDto.</returns>
// GET: api/Transaction/compte/5


// GET: api/Transaction/compte/5
/// <summary>
/// Récupère toutes les transactions liées à un compte donné.
/// </summary>
/// <param name="compteId">L'ID du compte.</param>
/// <returns>Liste de transactions avec leurs détails.</returns>
[HttpGet("compte/{compteId}")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionsByCompte(int compteId)
{
    var transactions = await _transactionService.GetTransactionsByCompteIdAsync(compteId);

    if (transactions == null || !transactions.Any())
        return NotFound($"Aucune transaction trouvée pour le compte ID {compteId}.");

    var transactionDtos = _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    return Ok(transactionDtos);
}














        // POST: api/Transaction
        /// <summary>
        /// Crée une nouvelle transaction et met à jour le solde du compte associé.
        /// </summary>
        /// <param name="transactionDto">Les données de création de la transaction.</param>
        /// <returns>La transaction créée (TransactionDto).</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Si CompteId, CategorieId ou DisciplineId est invalide
        public async Task<ActionResult<TransactionDto>> PostTransaction(TransactionCreateDto transactionDto)
        {
            try
            {
                var createdTransaction = await _transactionService.CreateTransactionAsync(transactionDto);
                
                // Le service retourne l'entité, mais nous avons besoin des détails (relations)
                // Le moyen le plus simple est de le re-récupérer avec les détails inclus
                var transactionWithDetails = await _transactionService.GetTransactionByIdAsync(createdTransaction.TransactionId);

                var readDto = _mapper.Map<TransactionDto>(transactionWithDetails);

                return CreatedAtAction(nameof(GetTransaction), new { id = readDto.TransactionId }, readDto);
            }
            catch (KeyNotFoundException ex)
            {
                // Attrape l'exception lancée si un ID de relation (Compte, Categorie, Discipline) est introuvable
                return NotFound(ex.Message); 
            }
        }

        // PUT: api/Transaction/5
        /// <summary>
        /// Met à jour complètement une transaction et corrige le solde du ou des comptes.
        /// </summary>
        /// <param name="id">L'ID de la transaction à mettre à jour.</param>
        /// <param name="transactionDto">Les nouvelles données de la transaction.</param>
        /// <returns>StatusCode 200 (OK) si la mise à jour est réussie.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutTransaction(int id, TransactionUpdateDto transactionDto)
        {
            // Correction : Suppression de la vérification id != transactionDto.TransactionId
            // car TransactionUpdateDto ne contient pas de TransactionId.
            // L'ID est passé directement au service.
            
            try
            {
                var updatedTransaction = await _transactionService.UpdateTransactionAsync(id, transactionDto);

                if (updatedTransaction == null)
                {
                    return NotFound($"Transaction avec ID {id} non trouvée.");
                }
                
                // Le service renvoie l'entité mise à jour avec les détails
                return Ok(_mapper.Map<TransactionDto>(updatedTransaction));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        // PATCH: api/Transaction/5
        /// <summary>
        /// Met à jour partiellement une transaction et corrige le solde du compte si le montant ou le compte change.
        /// </summary>
        /// <param name="id">L'ID de la transaction à mettre à jour.</param>
        /// <param name="patchDocument">Le document JSON Patch décrivant les changements.</param>
        /// <returns>StatusCode 200 (OK) avec le DTO mis à jour.</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TransactionDto>> PatchTransaction(
            int id, 
            [FromBody] JsonPatchDocument<TransactionUpdateDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest("Document de patch JSON invalide.");
            }
            
            var (transaction, success) = await _transactionService.PartialUpdateTransactionAsync(id, patchDocument);

            if (!success)
            {
                return NotFound($"Transaction avec ID {id} non trouvée ou dépendance invalide.");
            }
            
            return Ok(_mapper.Map<TransactionDto>(transaction));
        }

        // DELETE: api/Transaction/5
        /// <summary>
        /// Supprime une transaction et corrige le solde du compte.
        /// </summary>
        /// <param name="id">L'ID de la transaction à supprimer.</param>
        /// <returns>StatusCode 204 (No Content) si la suppression est réussie.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var isDeleted = await _transactionService.DeleteTransactionAsync(id);

            if (!isDeleted)
            {
                return NotFound($"Transaction avec ID {id} non trouvée.");
            }

            return NoContent();
        }
    }
}
