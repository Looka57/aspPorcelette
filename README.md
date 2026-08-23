# 🏛️ ASP Porcelette – Back-End

API REST développée avec **ASP.NET Core** pour alimenter le site de l'AS Porcelette.

Ce projet constitue le cœur de l'application. Il centralise les données, sécurise les accès et fournit l'ensemble des services utilisés par l'interface d'administration ainsi que par le site public.

---

## 🌐 Démonstration

Le projet est accessible en ligne :

**[Website](https://asporcelette-art-martiaux.fr/)**

---

## 🚀 Fonctionnalités

* 🔐 Authentification sécurisée avec JWT
* 👥 Gestion des utilisateurs et des rôles
* 📝 Gestion des actualités
* 📅 Gestion des événements
* 🥋 Gestion des activités et disciplines du club
* 🖼️ Gestion des contenus dynamiques
* 📂 Upload et gestion des fichiers
* 🩺 Gestion des certificats médicaux
* 📧 Envoi automatique des emails via Brevo
* 🔔 Rappels automatiques des certificats à J-30 et J-7
* 🚨 Notification automatique lors de l'expiration d'un certificat
* ⏰ Vérification quotidienne automatisée avec `BackgroundService`
* 🔄 API REST consommée par le Front-End Vue.js
* 🐳 Conteneurisation avec Docker
* 📊 Architecture évolutive et maintenable

---

## 🛠️ Technologies

* **ASP.NET Core**
* **C#**
* **Entity Framework Core**
* **SQL Server**
* **ASP.NET Core Identity**
* **JWT**
* **Swagger / OpenAPI**
* **Docker / Docker Compose**
* **Brevo SMTP**

---

## 🏗️ Architecture

```text
Vue.js Front-End
        │
        ▼
ASP.NET Core Web API
        │
        ├── ASP.NET Core Identity
        │
        ├── Services métier
        │
        ├── Background Services
        │
        └── Entity Framework Core
                    │
                    ▼
                SQL Server
```

L'API est conçue pour séparer les responsabilités entre les contrôleurs, les services métier et l'accès aux données.

---

## 📧 Système de rappels des certificats médicaux

Le back-end intègre un système automatisé de suivi des certificats médicaux des adhérents.

Lorsqu'un certificat médical est enregistré, sa date d'expiration est calculée automatiquement à **3 ans** à partir de sa date de délivrance.

Le système permet ainsi à l'administration de suivre la validité des certificats directement depuis l'application.

### 🔔 Rappels automatiques

Le back-end vérifie quotidiennement les certificats médicaux et déclenche automatiquement les notifications nécessaires :

* 📅 **J-30** : premier rappel avant expiration
* 📅 **J-7** : second rappel avant expiration
* 🚨 **Jour d'expiration** : notification d'expiration

Les emails sont envoyés via **Brevo SMTP**.

### ⚙️ Fonctionnement

Le système utilise un `BackgroundService` ASP.NET Core exécuté avec le back-end.

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
Analyse des dates d'expiration
       │
       ├── J-30 ──► Email Brevo
       │
       ├── J-7  ──► Email Brevo
       │
       └── Expiré ──► Email Brevo
```

Le service est exécuté automatiquement dans le conteneur Docker du back-end.

Le fuseau horaire `Europe/Paris` est configuré afin que les vérifications quotidiennes soient effectuées selon l'heure française.

La vérification est actuellement planifiée quotidiennement à **10h10**.

---

## 🐳 Docker

Le projet utilise Docker Compose afin d'exécuter les différents services de l'application :

```text
┌───────────────────────────────┐
│           Docker              │
│                               │
│  ┌─────────────┐              │
│  │   Front-End │ :8080        │
│  └──────┬──────┘              │
│         │                      │
│  ┌──────▼──────┐              │
│  │   Back-End  │ :5070        │
│  │ ASP.NET Core│              │
│  └──────┬──────┘              │
│         │                      │
│  ┌──────▼──────┐              │
│  │ SQL Server  │ :1433        │
│  └─────────────┘              │
│                               │
└───────────────────────────────┘
```

Le back-end et la base de données communiquent via un réseau Docker dédié.

Les informations sensibles telles que les mots de passe, clés JWT et identifiants SMTP sont fournies via des variables d'environnement et ne sont pas stockées directement dans le dépôt.

---

## ⚙️ Installation & Lancement

### Prérequis

* [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/)
* Git

### Cloner le dépôt

```bash
git clone https://github.com/Looka57/aspPorcelette.git
cd aspPorcelette
```

### Configuration

Créer et configurer les variables d'environnement nécessaires au fonctionnement de l'application.

Les paramètres sensibles comprennent notamment :

```text
SA_PASSWORD
JWT_KEY

BREVO_SMTP_HOST
BREVO_SMTP_PORT
BREVO_SMTP_USER
BREVO_SMTP_KEY
BREVO_FROM_EMAIL
BREVO_FROM_NAME
```

Les valeurs sensibles ne doivent jamais être versionnées dans Git.

### Lancer l'application avec Docker

```bash
docker compose up -d --build
```

Les migrations Entity Framework Core sont appliquées automatiquement au démarrage du back-end.

Pour consulter les logs du back-end :

```bash
docker compose logs -f backend
```

Pour arrêter les conteneurs :

```bash
docker compose down
```

---

## 📚 Documentation API

L'API utilise **Swagger / OpenAPI** pour documenter et tester les différents endpoints.

Une fois l'API lancée localement, Swagger est accessible depuis l'URL configurée par l'application.

---

## 🎯 Objectifs du projet

* Développer une API REST moderne.
* Mettre en place une architecture maintenable.
* Centraliser les données du site.
* Sécuriser les échanges entre le client et le serveur.
* Gérer les utilisateurs et leurs différents rôles.
* Automatiser certaines tâches administratives.
* Faciliter l'évolution future de l'application.
* Fournir une base technique adaptée à un déploiement en production.

---

## 💡 Compétences mises en œuvre

* API REST
* ASP.NET Core
* C#
* Entity Framework Core
* SQL Server
* ASP.NET Core Identity
* Authentification JWT
* Gestion des rôles et autorisations
* CRUD
* Validation des données
* Gestion des fichiers
* Services métier
* `BackgroundService`
* Automatisation des tâches
* Envoi d'emails SMTP
* Docker / Docker Compose
* Gestion des variables d'environnement
* Clean Code
* Déploiement en production

---

## 📂 Outils

* **IDE :** Visual Studio / Visual Studio Code
* **Base de données :** SQL Server
* **Documentation API :** Swagger / OpenAPI
* **Conteneurisation :** Docker
* **Gestion de versions :** Git / GitHub
* **Service d'envoi d'emails :** Brevo

---

## 🚀 État du projet

🟢 **Projet déployé et en production**

Le back-end est utilisé pour alimenter le site de l'AS Porcelette.

Le projet continue d'évoluer avec l'ajout de nouvelles fonctionnalités, améliorations techniques et automatisations destinées à faciliter la gestion quotidienne du club.
