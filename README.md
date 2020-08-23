# GameTracker Server

![.NET Core](https://github.com/tbd-friends/gametracker-mobile/workflows/.NET%20Core/badge.svg?branch=main)

API for tracking game collection, adding images, marking as favorite and tracking copies of games. 

AppSettings.json

```json
  "ConnectionStrings": {
    "gametracker": "<sql server connection string>"
  },
  "settings": {
    "tokenLength": 8
  },
  "images": {
    "local": true,
    "path": "images",
    "container": "<azure storage container name>",
    "storageurl": "<azure storage url>"
  },
  "auth": {
    "domain": "<domain to validate against Auth0>",
    "audience": "<audience to validate against Auth0>"
  }
```

### settings
- *tokenLength* - Length of the token to generate for friend requests

### images
- *local* - If **true** then the application directory is used, false will use the container
- *path* - If local, then the relative path to the images
- *container* - Name of the container used if using Azure Storage
- *storageUrl* - The url of the Azure Storage 

### auth
- *domain* - This is the domain you'll be given when configuring Auth0. This will match the domain on the JWT passed in.
- *audience* - This will be the audience provided in the Auth0 JWT token. 