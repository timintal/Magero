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
namespace {classSymbol.ContainingNamespace}
{{
public partial class {classSymbol.Name} 
{{
");

            foreach (IFieldSymbol fieldSymbol in fields)
            {
                ProcessField(source, fieldSymbol, attributeSymbol);
            }

            source.Append("\n\n}}");
            return source.ToString();
        }
    
        private void ProcessField(StringBuilder source, IFieldSymbol fieldSymbol, ISymbol attributeSymbol)
        {
            var fieldName = fieldSymbol.Name;
            ITypeSymbol fieldType = fieldSymbol.Type;

            AttributeData attributeData = fieldSymbol.GetAttributes().Single(ad =>
                ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));

            bool needDirty = ProcessAttribute(attributeData);

            string nameUpper = fieldName.Replace("_", "");
            char firstLetter = Char.ToUpper(nameUpper[0]);
            nameUpper = firstLetter + nameUpper.Substring(1, nameUpper.Length - 1);

            var text = extensionsSource
                .Replace("$type$", fieldType.ToDisplayString())
                .Replace("$NameUpper$", nameUpper)
                .Replace("$NameLower$", fieldName)
                .Replace("$NeedDirty$", (needDirty ? "IsDirty = true;" : ""));

            source.AppendLine(text);
        }

        private bool ProcessAttribute(AttributeData attributeData)
        {
            if (attributeData.ConstructorArguments.Length > 0)
            {
                return (bool) attributeData.ConstructorArguments[0].Value;
            }

            return false;
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
                    On$NameUpper$Changed?.Invoke($NameLower$, value);
                    $NameLower$ = value;
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