# discord-bot-csharp

`discord-bot-csharp` is a Discord bot written in C# using the DSharpPlus library. It helps manage assignments, delete messages, and much more.

## Features

-   Add an assignment
-   Delete an assignment
-   Update an assignment
-   Delete messages
-   Display bot status
-   Command documentation

## Prerequisites

-   [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
-   [Discord Developer Portal](https://discord.com/developers/applications) to obtain a bot token

## Installation

1. Clone the repository :

```sh
git clone https://github.com/Jul0P/discord-bot-csharp.git
cd discord-bot-csharp
```

2. Install dependencies :

```sh
dotnet restore
```

3. Rename the `.env.example` file to `.env` :

```sh
cp .env.example .env
```

4. Open the `.env` file and add your bot token :

```properties
TOKEN=your-discord-bot-token
```

## Usage

1. Build and run the project :

```sh
dotnet run
```

2. Invite the bot to your Discord server using the invitation link generated from the [Discord Developer Portal](https://discord.com/developers/applications)
