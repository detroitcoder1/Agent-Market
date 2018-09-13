Do not forget to change the properties on the resourcefile: Resources\ContactGenerator.dsrf
- Build Action: Content
- Copy to Output Directory: Copy if newer

It's also possible to move this file to a location you prefer.

Example Code:


{
  Generator gen = new Generator();
  gen.Settings.Language = Language.US;
  gen.DownloadLatestPackage();

  gen.RecordDefinition.AddFirstName("MyFieldFirstName");
  gen.RecordDefinition.AddLastName("MyLastName");

  if (gen.Generate(20))
  {
    gen.ExportOptions.IgnoreEmpty = true;
    string json = gen.Export(@"D:\output.json");
  }
}

Do you want to generate from your own resources?
No Problem: 

  gen.Settings.UseLocalResources = true;
  gen.Settings.PackageLocation = @"C:\PackageLocation";

=================================================================
Release History:
=================================================================

-----------------------------------------------------------------
1.3.51212
-----------------------------------------------------------------
[Added Fields]
- LorumIpsum, IBAN, BIC, Bank, Numbers, AccountNumber

[Added more resources]
See Wiki for amount

[Changed]
- Generate returns a ResultCode in stead of a Boolean

[Added Wiki]
- Wiki.html


-----------------------------------------------------------------
1.2.50129
-----------------------------------------------------------------
[HotFix]
- Export Options



-----------------------------------------------------------------
1.2.50128
-----------------------------------------------------------------
[Added Fields] 
- Color, Guid, HexNumber, HexColor, IPAddressV4, IPAddressV6, ISBN, 
  MACAddress, Password, PinCode, RowNumber, Time, Username

[Added Methods]
- SimpleGenerator: Static methods for single fields.

[Fix]
- SQL Export: DateTime2

1.1.50118
[Added Languages]
- support for counties: US,NL,BE

-----------------------------------------------------------------
1.0.41201
-----------------------------------------------------------------
[Added Export Formats]
- Json, XML and SQL export