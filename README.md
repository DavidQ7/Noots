# 📝 X Notes 🚀

Welcome to X Notes! Your ultimate note-taking solution. Create, edit, and organize your notes effortlessly.

## How to Install and Run Locally 🏠

### Angular Web 🅰️

Before you begin, make sure you have the following prerequisites installed:

- Node.js: v18.18.2+
- Angular CLI: 17.0.1+
- Package Manager: npm 9.8.1+

1. Open Web -> Backend -> WebAPI -> [Client](https://github.com/X-Notes/Web/tree/DEV/Backend/WebAPI/Client)
  1. Install packages run: **yarn**
  2. Run **yarn start**
  3. Open your browser and go to [localhost:4200](http://localhost:4200/)

     
### Backend .NET CORE | POSTGRESQL | Azure Storage | Redis (optional) 🖥️

Before you proceed, ensure you have the following prerequisites:

- .NET 8.0.100
- PostgreSQL
- Azure Storage (Azurite or cloud)

Here are the steps to set up the backend:

1. Open [appsettings.development.json](https://github.com/X-Notes/Web/blob/DEV/Backend/WebAPI/appsettings.Development.json)
2. Update the `DatabaseConnection` field with your database connection details.
3. If you don't have Redis, you can disable it by setting `Redis:Active` to `false`.
4. If you have your own Azure Blob Storage, update the `Azure:Storages` configuration with your values.
5. Clone the backend repository:  
5. Open Web -> [Backend](https://github.com/X-Notes/Web/tree/DEV/Backend)
6. Migrate the database using the command line:
   1. dotnet ef database update --project DatabaseContext\DatabaseContext.csproj --startup-project WebAPI\WebAPI.csproj --context DatabaseContext.ApiDbContext
   2. Or use your favorite IDE tools for database migration.
7. Run WebAPI project

## Support X Notes ☕

If you find X Notes helpful and would like to support our project, consider [buying us a coffee](https://www.buymeacoffee.com/xnotes). Your support is greatly appreciated!

## Contributing 🤝

We welcome your contributions to make X Notes even better! Here's how you can get involved:

1. **Track Bugs or Propose Features**: Visit our [Issues](https://github.com/X-Notes/Web/issues) page to report bugs or suggest new features. Your feedback is valuable to us!

2. **Code Contributions**:

   - To contribute code, start by creating a branch from the `DEV` branch.
   
   - Naming your branch is important to help us understand its purpose:
   
     - For Bug Fixes, use: `bugfix/github-issue-number`.
     
     - For New Features, use: `feature/github-issue-feature-number`.

   - Submit your pull request when your changes are ready into `DEV` branch, and we'll review it promptly.

Let's collaborate and make X Notes even more awesome together! 🚀👩‍💻👨‍💻

## Features

### Notes
![Screenshot 2024-01-14 164512](https://github.com/X-Notes/Web/assets/40700475/8557f607-ed17-43a0-8914-2ea50db8d35c)
![Screenshot 2024-01-14 163449](https://github.com/X-Notes/Web/assets/40700475/72c47caa-f41e-43a2-ad14-36765f98537e)

###  Folders
![Screenshot 2024-01-14 163823](https://github.com/X-Notes/Web/assets/40700475/ff1e7109-be53-4088-9d34-d6abdb82cac8)

### Base editor 
![Screenshot 2024-01-14 164729](https://github.com/X-Notes/Web/assets/40700475/8ff0f06c-4b1e-4a6e-a14d-8e4217a7d7da)
![Screenshot 2024-01-14 164542](https://github.com/X-Notes/Web/assets/40700475/512a5c9a-1726-4d04-817c-a1edf1c49349)
![Screenshot 2024-01-14 164309](https://github.com/X-Notes/Web/assets/40700475/a7cd1dea-3bcb-4313-bca5-6071b171c886)
![Screenshot 2024-01-14 163552](https://github.com/X-Notes/Web/assets/40700475/364104b7-de2a-453c-b4cc-6e1095be3f56)
![Screenshot 2024-01-14 164404](https://github.com/X-Notes/Web/assets/40700475/61372b1b-89d5-4ac4-9480-69443a36878e)

### Editor Audios
![Screenshot 2024-01-14 164421](https://github.com/X-Notes/Web/assets/40700475/52fed5c9-640d-499a-84bc-c4594bd26269)
![Screenshot 2024-01-14 163707](https://github.com/X-Notes/Web/assets/40700475/1f416700-43b7-4989-9bc8-dee1cc037f6f)

### Editor photos
![Screenshot 2024-01-14 164411](https://github.com/X-Notes/Web/assets/40700475/6760b63e-e290-4a47-a0c3-3adf0535f9b5)
![Screenshot 2024-01-14 163642](https://github.com/X-Notes/Web/assets/40700475/aebc5f3e-b3b3-429b-9e74-fcb8c0b53b3f)

###  Editor videos
![Screenshot 2024-01-14 163722](https://github.com/X-Notes/Web/assets/40700475/9bd358ed-6519-4d93-a47e-8565eb86d812)

### Editor documents
![Screenshot 2024-01-14 163650](https://github.com/X-Notes/Web/assets/40700475/d6b1a571-b485-492d-ab04-138e712cb656)

###  Notes Sharing
![Screenshot 2024-01-14 163944](https://github.com/X-Notes/Web/assets/40700475/6ee229fa-6a76-4495-8f6c-aba941b56670)

### Folders sharing
![image](https://github.com/X-Notes/Web/assets/40700475/07698ad3-21cb-4f85-b9ff-013c41fad785)

### Mobile Supported
![Screenshot 2024-01-14 164351](https://github.com/X-Notes/Web/assets/40700475/63391a21-5bee-46ff-b144-a26ff640b46b)
![Screenshot 2024-01-14 164345](https://github.com/X-Notes/Web/assets/40700475/c13000c3-85b5-42f0-b19c-ce790d59ac7a)

###  Advanced personalization settings
![Screenshot 2024-01-14 164258](https://github.com/X-Notes/Web/assets/40700475/7699f25a-2363-4de4-9165-2a19b6ea66ac)
User profile and feature for background changing
![Screenshot 2024-01-14 164249](https://github.com/X-Notes/Web/assets/40700475/b864b83e-2eff-451b-8a7b-a2173e1c91e0)

### Unathorized folder view if folder shared.
![Screenshot 2024-01-14 164205](https://github.com/X-Notes/Web/assets/40700475/71a84ec2-7536-4d68-87b0-3f939e14aa9d)

### Unathorized note view if note shared.
![Screenshot 2024-01-14 164029](https://github.com/X-Notes/Web/assets/40700475/325d768c-db0f-4296-bf1d-2ddb81df3b35)

###  Labels
![Screenshot 2024-01-14 163813](https://github.com/X-Notes/Web/assets/40700475/6cb5cf0b-bfff-4ba0-86c8-1517dd6f623a)

### Selection mode
![Screenshot 2024-01-14 163802](https://github.com/X-Notes/Web/assets/40700475/d5f4a158-d4ed-4115-90b0-f24f6f4731ca)

### Search
![Screenshot 2024-01-14 163750](https://github.com/X-Notes/Web/assets/40700475/0480c9b3-b45b-4715-8b3a-074acb28c2c4)

### White theme
![Screenshot 2024-01-14 163732](https://github.com/X-Notes/Web/assets/40700475/99194449-4023-401b-8606-55933843a284)

### Notifications
![Screenshot 2024-01-14 164758](https://github.com/X-Notes/Web/assets/40700475/1df4715b-39b7-4f75-ba4d-c09aba1dc171)

## Discussion 💬
Join the conversation about X Notes on [GitHub Discussions](https://github.com/X-Notes/Web/discussions). Share your thoughts, suggestions, and feedback with the community.

## License 📜
X Notes is distributed under the AGPLv3 License. See [LICENSE.md](https://github.com/X-Notes/Web/blob/DEV/LICENSE) for more information.

🎉 Happy Note-Taking with X Notes! 📝

