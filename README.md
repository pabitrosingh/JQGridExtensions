# JQGridExtensions
JQGrid Extensions for  Server Side Searching / Sorting 

## Landing Page of JQGrid
![JQGrid](https://github.com/pabitrosingh/JQGridExtensions/blob/master/JQGridScreen-1.PNG)


## Technology Used 

I have used open source Technologies / Framework for this projects 

* [JQGrid](http://www.guriddo.net/demo/guriddojs/)
* [Trirand JQGrid](http://trirand.com/blog/jqgrid/jqgrid.html)
* [ASP.NET MVC](https://docs.microsoft.com/en-us/aspnet/#pivot=aspnet)
* [Entity Framework](https://docs.microsoft.com/en-us/ef/)
* [LINQ](https://msdn.microsoft.com/en-in/library/bb308959.aspx)
* [Expression Tree](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/)
* [JQuery](https://jquery.com/)
* [JQuery UI](http://jqueryui.com/)

### Setup

this repo contains two projects in the folders 

> JQGridDemo

This Folder contains Domo app

> JQGridExtensions-DLL

This Folder contains DLL **JQGrdExtensions.dll** that is used in Demo app
[DLL File](https://github.com/pabitrosingh/JQGridExtensions/tree/master/JQGridExtensions-DLL/bin/Debug)

Sorting / Searching:
```
public JsonResult GetEmployeeCollection(JQGridSettings JQGrid)
{
    int TotapPages = 0;
    int TotalRecords = 0;
    IEnumerable<Employee> IEEmployee = null;
    IQueryable<Employee> IQEmployee = null;

    using (Employee DB = new Employee())
    {
        if (JQGrid._search)
        {
            //for Filter Toolbar 
            JQGridFilters _filters = JsonConvert.DeserializeObject<JQGridFilters>(JQGrid.filters);
            IQEmployee = DB.GetEmployeeColection().AsQueryable().JQGridToolBarSearch(_filters);

            //for Advanced Search
            //JQGridAdvanceFilter _filters1 = JsonConvert.DeserializeObject<JQGridAdvanceFilter>(JQGrid.filters);
            //IQEmployee = DB.GetEmployeeColection().AsQueryable().JQGridAdvancedSearch(_filters1);
        }
        else
        {
            IQEmployee = DB.GetEmployeeColection().AsQueryable().JQGridSorting(JQGrid.sidx, JQGrid.sord);
        }
        IEEmployee = IQEmployee.AsEnumerable();
        try
        {
            //Pagination
            TotapPages = Convert.ToInt32(Math.Ceiling((double)IEEmployee.Count() / (double)JQGrid.rows));
            TotalRecords = IEEmployee.Count();
            IEEmployee = IEEmployee.Skip((JQGrid.page - 1) * JQGrid.rows).Take(JQGrid.rows).ToList();
        }
        catch (Exception )
        {
            throw;
        }
    }
    var jsonReader = new
    {
        root = IEEmployee,
        page = JQGrid.page,
        total = TotapPages,
        records = TotalRecords
    };
    return Json(jsonReader, JsonRequestBehavior.AllowGet);
}
```


## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
