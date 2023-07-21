using System;
using System.IO;
using System.Collections.Generic;
using AutoMapper;
using Newtonsoft.Json;

public class TreeNodeData
{
    public string title { get; set; }
    public bool expanded { get; set; }
    public bool @checked { get; set; }
    public dynamic extra { get; set; }
    public List<TreeNodeData> children { get; set; }
    public int? totalChild { get; set; }

    public TreeNodeData()
    {
    }

    public TreeNodeData(string title, bool expanded, bool @checked, List<TreeNodeData> children, int? totalChild = null, dynamic extra = null)
    {
        this.title = title;
        this.expanded = expanded;
        this.@checked = @checked;
        this.children = children;
        this.totalChild = totalChild;
        this.extra = extra;
    }
}

public class ProductCategory
{
    public int? id { get; set; }
    public string? code { get; set; }
    public string? name { get; set; }
    public bool? isActive { get; set; }
    public int? idParentCategory { get; set; }
    public int? totalChild { get; set; }
    public List<ProductCategory>? children { get; set; }

    public ProductCategory(int? id, string? code, string? name, bool? isActive, int? idParentCategory, int? totalChild, List<ProductCategory>? children)
    {
        this.id = id;
        this.code = code;
        this.name = name;
        this.isActive = isActive;
        this.idParentCategory = idParentCategory;
        this.totalChild = totalChild;
        this.children = children;
    }
}

class Program
{
    static void Main()
    {

        MapperConfiguration configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ProductCategory, TreeNodeData>()
                .ForMember(dest => dest.title, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.extra, opt => opt.MapFrom(src => src.id));
        });


        IMapper mapper = configuration.CreateMapper();

        string jsonFilePath = "DataSet/Data.json";
        string jsonContent = File.ReadAllText(jsonFilePath);

        ProductCategory[] categories = JsonConvert.DeserializeObject<ProductCategory[]>(jsonContent);

        foreach (ProductCategory category in categories)
        {
            TreeNodeData treeNodeData = mapper.Map<TreeNodeData>(category);
            PrintCategoryTree(treeNodeData, 0);
        }
    }

    static void PrintCategoryTree(TreeNodeData category, int level)
    {
        string i = new string(' ', level * 5);
        Console.WriteLine($"{i}{category.extra},{category.title} ");

        if (category.children != null && category.children.Count > 0 && category.totalChild > 0)
        {
            foreach (TreeNodeData child in category.children)
            {
                PrintCategoryTree(child, level + 1);
            }
        }
    }
}