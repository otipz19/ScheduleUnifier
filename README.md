# Schedule Unifier

CLI Application that allows user to format NaUKMA schedule into JSON.

## Features

- Parsing of __.docx__ and __.xlsx__ formats.*
- Serialization to __JSON__.
- Retrieving files via __HTTP__.
- Simple command-line interface.

_*Binary formats like .doc are currently unsupported._

## Usage guide

1. Download appropriate archive from Releases page
2. Extract it to any empty folder
3. Open folder in your favorite CLI shell
4. Run __setup__ command to create default configuration file and input/output folders:

```sh
ScheduleUnifier.exe setup
```

5. Run __run__ command to start application with default configuration. It will use HTTP to download files with schedules from my.ukma.edu.ua website:

```sh
ScheduleUnifier.exe run
```

6. You can change application configuration using __config__ command or by manually editing __config.json__ file. __config__ command has 4 arguments:
    - __--input__ [input folder path]:
    to set folder, where files will be loaded via HTTP and from where they will be retrieved by parser. If HTTP mode is disabled, files have to be placed manually to this folder
    - __--output__ [output folder path]:
    to set folder, where output.json file will be placed.
    - __--http__ [true/false]:
    to set HTTP-loading mode
    - __--urls__ [urls list]:
    to set list of urls, that will be used, when HTTP mode is enabled.

```sh
ScheduleUnifier.exe config --input [input folder path] --output [output folder path] --http [true/false] --urls [urls list]
```

## Technologies used

- Languages:
  - __C#__
- Libraries:
  - __System.CommandLine__ for CLI implementation
  - __OpenXML__ for .docx parsing
  - __EPPlus__ for .xlsx parsing
  - __NPOI__ for .doc parsing _(currently doesn't work)_
  - __b2xtranslator__ for .doc to .docx converting _(currently doesn't work)_
