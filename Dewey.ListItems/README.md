List Command
===
The `dewey list` command will try find a dewey manifest file in the current directory. Any other manifest files referenced will also be loaded.  
Every component and runtime-resource described in all the discovered manifest files are listed with their type and the path of the manifest file they were loaded from.

# Example
```
Looking for a manifest file in the current working directory.
Found manifest file: D:\Development\Dewey-Example\test-repo\dewey.json
ExampleWebApiComp (web-ui) - "D:\Development\Dewey-Example\test-repo\ExampleWebApiComp\"
ExampleWebApiComp2 (web-api) - "D:\Development\Dewey-Example\test-repo\ExampleWebApiComp2\"
ExampleWebApiComp6 (web-service) - "D:\Development\Dewey-Example\test-repo\ExampleWebApiComp6\"
ExampleWebApiComp7 (web) - "D:\Development\Dewey-Example\test-repo\ExampleWebApiComp7\src\WebApplication2"
ExampleAgent (executable-worker) - "D:\Development\Dewey-Example\test-repo\ExampleAgent\src"
incoming (queue) - "D:\Development\Dewey-Example\test-repo\ExampleAgent\src"
status (queue) - "D:\Development\Dewey-Example\test-repo\ExampleAgent\src"
queue-host (environment-variable) - "D:\Development\Dewey-Example\test-repo\ExampleAgent\src"
Config.xml (file) - "D:\Development\Dewey-Example\test-repo\ExampleAgent\src"
databaseA (database) - "D:\Development\Dewey-Example\test-repo\ExampleAgent\src"
databaseA-connection (environment-variable) - "D:\Development\Dewey-Example\test-repo\ExampleAgent\src"
rocketship (queue) - "D:\Development\Dewey-Example\test-repo\ExampleAgent\src"
```
