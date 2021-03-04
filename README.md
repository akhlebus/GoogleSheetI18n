# Introduction 
Many projects add support of i18n (internationalization) at some stage. There are multiple ways how engineers manage translations from implementation point of view, but translation teams often use Google Sheets for some reason. Engineers usually carry these translations on their own.

As software engineers, we decided to try using Google Sheets for translations directly, as the only source of truth. With caching, fallback, and hopefully no pain.

This POC (Proof of concept) uses the following Google Sheet document as the source: [Translations POC](https://docs.google.com/spreadsheets/d/156UC1_Y2mwhGfY7Y-8RDiNM75eavFwzxBFNP9emNTzc/edit)

Each tab represents a namespace, and expected to contain transtations in the following format:

ID | language-tag | other-language-tag | ...
-|-|-|-
some.id | Some translation | Some other translation | ... 
another.id | Another translation | Another translation | ...

For example:

ID | en-US | en-GB
-|-|-
color.gray | Color: gray  | Colour: grey  
word.flavor | Flavor | Flavour
word.analyze | Analyze | Analyse

# Getting Started
## Prereqisites
- [.NET 5.0 SDK](https://dotnet.microsoft.com/download/dotnet/5.0) to build the app
- [Google developer account](https://console.developers.google.com) to run the app against Google Sheets

## Build the app
There is no specia dependencies or caveats. So just restore Nuget packages before the build, and you should be fine.

## Run the app
There are a few thing you should know before you start.

We expect a valid Google Sheet ID in `SPREADSHEET_ID` environment variable.

We use [Google cloud (service) credentials](https://cloud.google.com/docs/authentication/production) for accessing Google Sheets, so you should provide them from your side. There are a few options:
- Via setting credentials encoded json into `GOOGLE_APPLICATION_CREDENTIALS_AS_JSON` environment variable;
- Via setting path to file with credentials to `GOOGLE_APPLICATION_CREDENTIALS` environment variable;
- Via setting path to file with credentails to `CredentialsFilePath` variable in `appsettings.json`.

We currently use local file system for backups (local cache) as the only option, so let us know the folder via `BACKUP_FOLDER_PATH` envronment variable.

We can also purge the cache via a subscription, which leaves on the endpoint, which is configured with the following environment variables:
`SUBSCRIPTION_URL` - absolute URL of the endpoint to purge the cache;
`EMPTY_CACHE_TOKEN` - the token we expect to avoid confusion.

Check more options in `GoogleSheetI18n.Api.SimpleWebApi\Properties\launchSettings.json`

## Startup

There's currently the only ASP.NET Core project to be run, which serves both API and static frontend app: `GoogleSheetI18n.Api.SimpleWebApi`.

## Interested or want to contribute?

Please feel free to contact:
- Dzmitry Shubkin
- Aliaksandr Khlebus