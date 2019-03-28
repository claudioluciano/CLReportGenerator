# CLReportGenerator
A Simple report generator (PDF, Excel)
[![nuget](https://img.shields.io/badge/nuget-download-blue.svg)](https://www.nuget.org/packages/Realm.Json.Extensions/)

##### How To Use

###### Startup.cs
```csharp
   public void ConfigureServices(IServiceCollection services)
   {
   //cool stuff...
	   services.AddReportGenerator(Configuration.GetConnectionString("LocalConnection"));
	   
	//more cool stuff...
   }
```
Then just inject the **IReportGenerator** 
###### Startup.cs
```csharp
private readonly IReportGenerator _reportGenerator;

public ValuesController(IReportGenerator reportGenerator)
{
	_reportGenerator = reportGenerator;
}
```
For Excel Report **GenerateReportAsEXCEL**
```csharp
 // GET api/values
 [HttpGet]
 public async Task<IActionResult> Get()
{
	var query = "SELECT * FROM Pessoa";
	return await _reportGenerator.GenerateReportAsEXCEL<Pessoa>("Relatorio", query);
}
```
For PDF Report **GenerateReportAsPDF**
```csharp
 // GET api/values
 [HttpGet]
 public async Task<IActionResult> Get()
{
	var query = "SELECT * FROM Pessoa";
	return await _reportGenerator.GenerateReportAsPDF<Pessoa>("Relatorio", "Report/PessoaReport", query);
}
```
PDF report you will need a view (Razor View)

Like that
```csharp
@model IEnumerable<CL.ReportGenerator.Demo.Model.Pessoa>

@{
    Layout = null;
}

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>PessoaReport</title>
</head>
<body>
<p>
    <h1>Teste Report</h1>
</p>
<table class="table">
    <thead>
        <tr>
            <th>
                @Html.DisplayNameFor(model => model.Id)
            </th>

            <th>
                @Html.DisplayNameFor(model => model.Nome)
            </th>
        </tr>
    </thead>
    <tbody>
        @foreach (var item in Model)
        {
            <tr>
                <td>
                    @Html.DisplayFor(modelItem => item.Id)
                </td>

                <td>
                    @Html.DisplayFor(modelItem => item.Nome)
                </td>
            </tr>
        }

        @for (int i = 0; i < 10000; i++)
        {
            <tr>
                <td>
                    @Html.DisplayFor(modelItem => i)
                </td>
            </tr>
        }
    </tbody>
</table>
</body>
</html>
```
