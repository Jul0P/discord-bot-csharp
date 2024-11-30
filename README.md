# discord-bot-csharp

`discord-bot-csharp` est un bot Discord écrit en C# utilisant la bibliothèque DSharpPlus. Il permet de gérer des devoirs, de supprimer des messages, et bien plus encore.

## Fonctionnalités

-   Ajouter un devoir
-   Supprimer un devoir
-   Mettre à jour un devoir
-   Supprimer des messages
-   Afficher le statut du bot
-   Documentation des commandes

## Prérequis

-   [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
-   [Discord Developer Portal](https://discord.com/developers/applications) pour obtenir un token de bot

## Installation

1. Clonez le dépôt :

```sh
git clone https://github.com/votre-utilisateur/discord-bot-csharp.git
cd discord-bot-csharp
```

2. Installez les dépendances :

```sh
dotnet restore
```

3. Renommez le fichier `.env.example` en `.env` :

```sh
cp .env.example .env
```

4. Ouvrez le fichier `.env` et ajoutez votre token de bot :

```properties
TOKEN=your-discord-bot-token
```

## Utilisation

1. Compilez et exécutez le projet :

```sh
dotnet run
```

2. Invitez le bot sur votre serveur Discord en utilisant le lien d'invitation généré depuis le [Discord Developer Portal](https://discord.com/developers/applications).
