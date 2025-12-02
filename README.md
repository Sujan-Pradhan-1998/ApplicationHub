# ApplicationHub
Application Hub can be used for storing the user application from details. This project uses Repository Design Pattern.

# Prerequisites
- .Net SDK (version 8.0)
- Node js version 22

# Technologies
- .NET Core 8.0
- React 18
- Sqlite Db

## Improvement Required:
- Redis cache can be used instead
- Different database could be used for storing the data.
- More unit test case is Required
- Email for registering new user could be sent to email for confirmation purpose
- Roles could be implemented for better control process of the overall user application.

## List of Assumption :
- The user logged in is only allowed view their applications.


## Restore Dependencies:
Run the following command to restore all .NET dependencies:
- Navigate to ApplicationHub.Api folder

```bash
dotnet restore
```

Run the following to install node_modules
- Navigate to ApplicationHub.Api/ClientApp folder
```bash
npm install
```

## Running the Application
You can run the application api using the .NET CLI:
- Navigate to ApplicationHub.Api
```bash
dotnet run
```

You can run the application UI using the npm:
- Navigate to ApplicationHub.Api/ClientApp
```bash
npm run dev
```

You can use following test user:
- Email: test@gmail.com 
- Password: Test123