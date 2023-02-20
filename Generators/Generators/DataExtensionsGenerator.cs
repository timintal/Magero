using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace DataClassExtensions
{
    [Generator]
    public class DataExtensionsGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // context.RegisterForPostInitialization
            //     (i => i.AddSource("DataFieldAttribute.g.cs", _attributeText));
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
            
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxContextReceiver is SyntaxReceiver receiver))
                return;

            var groups = receiver.Fields
                .GroupBy<IFieldSymbol, INamedTypeSymbol>(f => f.ContainingType,
                    SymbolEqualityComparer.Default);

            INamedTypeSymbol attributeSymbol = context.Compilation.GetTypeByMetadataName("_Game.DataExtension.DataFieldAttribute");

            foreach (IGrouping<INamedTypeSymbol, IFieldSymbol> group in groups)
            {
                var classSource = ProcessClass(group.Key, group, attributeSymbol);
                
                context.AddSource($"{group.Key.Name}_Components_g.cs", SourceText.From(classSource, Encoding.UTF8));
            }
        }
        
        private string ProcessClass(INamedTypeSymbol classSymbol, IEnumerable<IFieldSymbol> fields,
            ISymbol attributeSymbol)
        {
            var source = new StringBuilder($@"
using Newtonsoft.Json;
namespace {classSymbol.ContainingNamespace}
{{
public partial class {classSymbol.Name} 
{{
");         bool needAddUpgradeEvent = false;

            foreach (IFieldSymbol fieldSymbol in fields)
            {
                ProcessField(source, fieldSymbol, attributeSymbol, out bool generateUpgradeEvent);
                needAddUpgradeEvent |= generateUpgradeEvent;
            }

            if (needAddUpgradeEvent)
            {
                source.AppendLine("public event System.Action OnPlayerParamUpgraded;");
            }
            source.Append("\n\n}}");
            return source.ToString();
        }
    
        private void ProcessField(StringBuilder source, IFieldSymbol fieldSymbol, ISymbol attributeSymbol, out bool generateUpgradeEvent)
        {
            var fieldName = fieldSymbol.Name;
            ITypeSymbol fieldType = fieldSymbol.Type;

            AttributeData attributeData = fieldSymbol.GetAttributes().Single(ad =>
                ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));

            ProcessAttribute(attributeData, out bool needDirty, out bool isUpgrade, out bool createPresentedCounterpart);

            string nameUpper = fieldName.Replace("_", "");
            char firstLetter = Char.ToUpper(nameUpper[0]);
            nameUpper = firstLetter + nameUpper.Substring(1, nameUpper.Length - 1);

            generateUpgradeEvent = isUpgrade;
            
            var text = extensionsSource
                .Replace("$type$", fieldType.ToDisplayString())
                .Replace("$NameUpper$", nameUpper)
                .Replace("$NameLower$", fieldName)
                .Replace("$NeedDirty$", (needDirty ? "IsDirty = true;" : ""))
                .Replace("$IsUpgradeMade$", (isUpgrade ? "OnPlayerParamUpgraded?.Invoke();" : ""));

            source.AppendLine(text);

            if (createPresentedCounterpart)
            {
                
                source.AppendLine($"\t\t[JsonProperty] protected {fieldType.ToDisplayString()} {fieldName}Presented;");
                
                var presentedFieldText = extensionsSource
                    .Replace("$type$", fieldType.ToDisplayString())
                    .Replace("$NameUpper$", nameUpper + "Presented")
                    .Replace("$NameLower$", fieldName + "Presented")
                    .Replace("$NeedDirty$", ("IsDirty = true;"))
                    .Replace("$IsUpgradeMade$", (isUpgrade ? "OnPlayerParamUpgraded?.Invoke();" : ""));

                source.AppendLine(presentedFieldText);
            }
        }

        private void ProcessAttribute(AttributeData attributeData, out bool needDirty, out bool isUpgrade, out bool createPresentedCounterpart)
        {
            needDirty = (bool) attributeData.ConstructorArguments[0].Value;
            isUpgrade = (bool) attributeData.ConstructorArguments[1].Value;
            createPresentedCounterpart = (bool) attributeData.ConstructorArguments[2].Value;
        }
            
        
        const string extensionsSource =@"
        public event System.Action<$type$, $type$> On$NameUpper$Changed;
        public $type$ $NameUpper$
        {
            get => $NameLower$;
            set
            {
                if ($NameLower$ != value)
                {
                    $NeedDirty$
                    $type$ oldValue = $NameLower$;
                    $NameLower$ = value;
                    On$NameUpper$Changed?.Invoke(oldValue, value);
                    $IsUpgradeMade$
                }
            }
        }
";
    }
    
    
    public class SyntaxReceiver : ISyntaxContextReceiver
    {
        public List<IFieldSymbol> Fields { get; } = new List<IFieldSymbol>();
        public List<string> FieldNames { get; } = new List<string>();
        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is FieldDeclarationSyntax fieldDeclarationSyntax
                && fieldDeclarationSyntax.AttributeLists.Count > 0)
            {
                foreach (VariableDeclaratorSyntax variable in fieldDeclarationSyntax.Declaration.Variables)
                {
                    IFieldSymbol fieldSymbol = context.SemanticModel.GetDeclaredSymbol(variable) as IFieldSymbol;

                    if (fieldSymbol.GetAttributes()
                    .Any(ad => ad.AttributeClass.ToDisplayString() == "_Game.DataExtension.DataFieldAttribute"))
                    {
                        Fields.Add(fieldSymbol);
                    }
                }
            }
            
        }
    }
}