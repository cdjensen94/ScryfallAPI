ScryfallAPI
A .NET 8 API Service connecting remotely to retrieve data

How it works?
This API connects remotely to an Open Source website (Scryfall) providing data about Magic cards showing only cards that fits a certain criteria, that is the cards have to have a high 'Penny Rank'. Those with a low ranking, will not be retrieved by this API.

The first time a user is connecting to the API, they will be asked to Login using their email, once inserted, that email will be stored in a database. Once into in the Index page, the user will also be able to mark as favorites the cards he wishes to add.
