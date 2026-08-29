Oui. Pour le **README du back-end**, je ferais la même mise à jour que pour le front, mais en mettant davantage en avant ce qu'on a réellement ajouté récemment : **gestion des adhérents, renouvellements par saison, certificats médicaux, rôles, statistiques, comptabilité et logique de saison sportive**.

Je corrigerais également un point important : dans ton README actuel tu annonces les **rappels automatiques J-30/J-7 et l'expiration**, donc si cette partie est bien présente dans ton back, on la conserve.

# 🏛️ AS Porcelette Arts Martiaux — Back-End

API REST développée avec **ASP.NET Core** pour alimenter l'application web de gestion de l'association **AS Porcelette Arts Martiaux**.

Le back-end constitue le cœur de l'application. Il centralise les données, sécurise les accès et fournit les services utilisés par le site public ainsi que par les interfaces d'administration et des adhérents.

---

## 🌐 Démonstration

Le projet est accessible en ligne :

**[https://asporcelette-art-martiaux.fr/](https://asporcelette-art-martiaux.fr/)**

---

## 🚀 Fonctionnalités

### 🔐 Authentification et utilisateurs

* Authentification sécurisée avec JWT
* Gestion des utilisateurs avec ASP.NET Core Identity
* Gestion des rôles et autorisations
* Gestion des profils utilisateurs
* Protection des endpoints
* Gestion des accès selon le rôle

Profils actuellement pris en charge :

* **Administrateur**
* **Sensei**
* **Adhérent**
* **Comptable**

---

### 👥 Gestion des adhérents

* Création d'adhérents
* Consultation des adhérents
* Modification des informations personnelles
* Gestion des coordonnées
* Gestion des disciplines
* Gestion des grades
* Gestion des photos de profil
* Gestion du statut de l'adhérent
* Gestion des dates d'adhésion
* Gestion des dates de renouvellement
* Gestion des informations de profil

---

### 🔄 Gestion des renouvellements

L'application permet de gérer les renouvellements des adhérents selon le fonctionnement saisonnier de l'association.

Une saison sportive est définie du :

**1er septembre → 30 juin**

Exemple :

**Saison 2026-2027 : 01/09/2026 → 30/06/2027**

Le système permet notamment de :

* suivre les adhérents à renouveler ;
* enregistrer les renouvellements ;
* distinguer les adhérents renouvelés des adhérents non renouvelés ;
* associer les renouvellements à une saison sportive ;
* alimenter les statistiques de la saison.

---

### 🥋 Gestion des disciplines

Gestion des disciplines proposées par l'association :

* Judo
* Aïkido
* Ju-jitsu
* Judo Détente

Les adhérents peuvent être associés à leur discipline afin de permettre leur affichage et leur suivi dans l'application.

---

### 🩺 Gestion des certificats médicaux

Le back-end permet de gérer les informations relatives aux certificats médicaux des adhérents.

Les données enregistrées comprennent notamment :

* certificat médical fourni ou non ;
* date du certificat médical ;
* date d'expiration ;
* date de rappel.

La durée de validité actuellement utilisée est de **3 ans** à partir de la date du certificat.

La date d'expiration est calculée automatiquement par l'application.

---

### 📧 Notifications et rappels

Le back-end intègre un système automatisé permettant de surveiller les dates d'expiration des certificats médicaux.

Des notifications peuvent être déclenchées automatiquement :

* **J-30** avant expiration ;
* **J-7** avant expiration ;
* **Jour d'expiration**.

Les emails sont envoyés via **Brevo SMTP**.

---

### ⏰ BackgroundService

La surveillance des certificats est automatisée grâce à un `BackgroundService` ASP.NET Core.

```text
BackgroundService
       │
       ▼
Vérification quotidienne
       │
       ▼
SQL Server
       │
       ▼
Analyse des dates
       │
       ├── J-30 ──► Email
       │
       ├── J-7  ──► Email
       │
       └── Expiré ──► Email
```

Le service fonctionne directement avec le back-end et permet d'automatiser les tâches ne nécessitant pas d'intervention manuelle.

Le fuseau horaire **Europe/Paris** est pris en compte pour les traitements planifiés.

---

### 📰 Gestion des contenus

L'API permet également de gérer les contenus du site :

* Actualités
* Événements
* Activités
* Disciplines
* Contenus dynamiques
* Informations utilisées par le site public

---

### 📂 Gestion des fichiers

Le back-end prend en charge la gestion des fichiers utilisés par l'application, notamment les images et photos de profil.

Les fichiers sont associés aux données correspondantes et accessibles depuis le front-end.

---

### 💰 Gestion comptable

Le back-end intègre également les fonctionnalités nécessaires au suivi comptable de l'association.

Les données comptables sont organisées autour du fonctionnement par **saison sportive**, avec notamment :

* gestion des dépenses ;
* dates d'exécution ;
* suivi des opérations ;
* association des données à une période comptable.

---

### 📊 Statistiques

L'API fournit les données nécessaires au tableau de bord d'administration.

Les statistiques permettent notamment de suivre :

* le nombre d'adhérents ;
* la répartition par discipline ;
* les renouvellements ;
* les certificats médicaux ;
* l'évolution des inscriptions.

Ces données sont consommées par le front-end afin d'alimenter les compteurs et graphiques du tableau de bord.

---

## 🛠️ Technologies

* **ASP.NET Core**
* **C#**
* **Entity Framework Core**
* **SQL Server**
* **ASP.NET Core Identity**
* **JWT**
* **Swagger / OpenAPI**
* **Docker**
* **Docker Compose**
* **Brevo SMTP**
* **BackgroundService**

---

## 🏗️ Architecture

```text
Vue.js Front-End
        │
        ▼
ASP.NET Core Web API
        │
        ├── Controllers
        │
        ├── Services métier
        │
        ├── ASP.NET Core Identity
        │
        ├── Background Services
        │
        └── Entity Framework Core
                    │
                    ▼
                SQL Server
```

L'API sépare les différentes responsabilités de l'application afin de faciliter sa maintenance et son évolution.

---

## 🗄️ Gestion des données

L'accès aux données est assuré par **Entity Framework Core** avec une base de données **SQL Server**.

Les principales données gérées comprennent notamment :

* utilisateurs ;
* adhérents ;
* disciplines ;
* actualités ;
* événements ;
* informations comptables ;
* certificats médicaux ;
* renouvellements.

ASP.NET Core Identity est utilisé pour la gestion des comptes, des mots de passe et des rôles.

---

## 🐳 Docker

Le projet utilise **Docker Compose** afin d'exécuter les différents services de l'application.

```text
┌─────────────────────────────────┐
│             Docker              │
│                                 │
│  ┌───────────────┐              │
│  │   Front-End   │ :8080        │
│  └───────┬───────┘              │
│          │                       │
│  ┌───────▼───────┐              │
│  │   Back-End    │ :5070        │
│  │ ASP.NET Core  │              │
│  └───────┬───────┘              │
│          │                       │
│  ┌───────▼───────┐              │
│  │  SQL Server   │ :1433        │
│  └───────────────┘              │
│                                 │
└─────────────────────────────────┘
```

Le back-end et SQL Server communiquent via le réseau Docker.

Les informations sensibles ne sont pas stockées dans le dépôt Git.

Elles sont fournies via des variables d'environnement, notamment :

* chaîne de connexion SQL Server ;
* clé JWT ;
* identifiants SMTP ;
* paramètres de messagerie.

---

## ⚙️ Installation & lancement

### Prérequis

* .NET 8.0 SDK
* Docker Desktop
* Git

### Cloner le dépôt

```bash
git clone https://github.com/Looka57/aspPorcelette.git
cd aspPorcelette
```

### Configuration

Les paramètres sensibles nécessaires au fonctionnement de l'application doivent être configurés via les variables d'environnement.

Les valeurs sensibles ne doivent jamais être versionnées dans Git.

### Lancer avec Docker

```bash
docker compose up -d --build
```

Les migrations Entity Framework Core sont appliquées automatiquement au démarrage du back-end.

### Consulter les logs

```bash
docker compose logs -f backend
```

### Arrêter les conteneurs

```bash
docker compose down
```

---

## 📚 Documentation API

L'API utilise **Swagger / OpenAPI** afin de documenter et tester les différents endpoints.

Une fois l'API lancée localement, Swagger est accessible depuis l'URL configurée par l'application.

---

## 🎯 Objectifs du projet

* Développer une API REST moderne.
* Centraliser les données de l'association.
* Sécuriser les échanges entre le client et le serveur.
* Gérer les utilisateurs et leurs rôles.
* Faciliter la gestion des adhérents.
* Automatiser le suivi des certificats médicaux.
* Gérer les renouvellements selon les saisons sportives.
* Fournir les données nécessaires aux statistiques.
* Centraliser certaines fonctions administratives et comptables.
* Construire une architecture maintenable et évolutive.
* Fournir une base technique adaptée à un déploiement en production.

---

## 💡 Compétences mises en œuvre

### Développement back-end

* ASP.NET Core
* C#
* API REST
* Entity Framework Core
* SQL Server
* CRUD
* Services métier
* Validation des données

### Authentification et sécurité

* ASP.NET Core Identity
* JWT
* Gestion des rôles
* Autorisation
* Protection des endpoints

### Automatisation

* `BackgroundService`
* Vérifications planifiées
* Gestion des dates d'expiration
* Notifications automatiques
* Envoi d'emails SMTP

### Gestion métier

* Gestion des adhérents
* Gestion des disciplines
* Gestion des renouvellements
* Gestion des saisons sportives
* Gestion des certificats médicaux
* Gestion comptable
* Statistiques

### Infrastructure

* Docker
* Docker Compose
* Variables d'environnement
* SQL Server
* Déploiement en production

### Documentation et maintenance

* Swagger / OpenAPI
* Git / GitHub
* Clean Code
* Architecture évolutive

---

## 📂 Outils

* **IDE :** Visual Studio / Visual Studio Code
* **Framework :** ASP.NET Core
* **Langage :** C#
* **ORM :** Entity Framework Core
* **Base de données :** SQL Server
* **Documentation API :** Swagger / OpenAPI
* **Conteneurisation :** Docker / Docker Compose
* **Gestion de versions :** Git / GitHub
* **Emails :** Brevo SMTP

---

## 🚀 État du projet

🟢 **Projet déployé et en production**

Le back-end est actuellement utilisé pour alimenter le site de l'**AS Porcelette Arts Martiaux**.

L'application continue d'évoluer avec l'ajout de nouvelles fonctionnalités, améliorations techniques et automatisations destinées à faciliter la gestion quotidienne de l'association.

---

## 👩‍💻 Projet réalisé par

**Amandine Napolitano**

Développeuse Web

Projet réalisé dans le cadre de la conception et du développement d'une application web complète pour la gestion d'une association sportive.
