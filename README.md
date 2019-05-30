# ConfigurationGenerator


### What's this?
This is just a simple Json Configuration File Generator given a class definition and a resolver class that retrieve values stored in the
json configuration files.

Once installed the ConfigurationGenerator.nupkg will be added two folder to your project:
```
- ConfigFiles
  - Data
  - Schema
```
You most create your configuration class ClassName.cs inside Schema folder, once there your go to Package Manager Console and type 
New-Config ClassName, this will generate ClassName.json file inside Data Folder. Now you can call ConfigResolver singleton class to 
retrieve the property wished from the given class.

_Any observation or contribution will be welcome._
***

### ConfigurationGenerator Usage Example
```
- ConfigFiles
  - Data
  - Schema
    - Info.cs
 ``` 
  Let define Info.cs:
  ```
  public class Info
  {
      public string Name { get; set; } = "Last";
      public Guid Guid { get; set; } = Guid.NewGuid();
  }
```

Now you go to Package Manager Console and type:
```
New-Config Info
```

This will generate a Info.json file, as consequence your final project will look like:
```
- ConfigFiles
  - Data
    - Info.json
  - Schema
    - Info.cs
```
Info.json data:
```
{
  "Name": "Last",
  "Guid": "f61db2da-4b64-4e99-b214-889fd297ac25"
}
```

Now if you want to get the Name Property Value you have to put in your code:
```
using ConfigFileGenerator.Implementation;

string name = ConfigFileGenerator.Implementation.ConfigResolver.Instance.Resolve<Info, string>(x => x.Name);
Guid guid = ConfigFileGenerator.Implementation.ConfigResolver.Instance.Resolve<Info, Guid>(x => x.Guid);
```
If you want to retrieve the whole entity use instead:
```
Info info = ConfigFileGenerator.Implementation.ConfigResolver.Instance.Resolve<Info>();
```
