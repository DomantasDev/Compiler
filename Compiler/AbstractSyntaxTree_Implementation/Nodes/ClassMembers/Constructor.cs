using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using AbstractSyntaxTree_Implementation.ResolveNames;
using static AbstractSyntaxTree_Implementation.Extensions;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers
{
    public class Constructor : ClassMember
    {
        public TokenNode Visibility { get; set; }
        public List<Parameter> Parameters { get; set; }
        public List<Expression> BaseArguments { get; set; }
        public Body Body { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Visibility), Visibility);
            p.Print(nameof(Parameters), Parameters);
            p.Print(nameof(BaseArguments), BaseArguments);
            p.Print(nameof(Body), Body);
        }

        public override void ResolveNames(Scope scope)
        {
            Parameters?.ForEach(x => x.ResolveNames(scope));
            BaseArguments?.ForEach(x => x.ResolveNames(scope));
            Body?.ResolveNames(new Scope(scope));
        }

        public override void AddName(Scope scope)
        {
            var className = FindAncestor<Class>().Name.Value;
            if(className == null)
                Console.WriteLine($"Constructor can only be defined in a class. Line {Visibility.Line}");

            var paramTypes = Parameters?.Select(x => x.Type.Value);

            var name = new Name(new TokenNode
            {
                Value = CreateConstructorName(className, paramTypes),
                Line = Visibility.Line
            }, NameType.Method);

            scope.Add(name, this);
        }

        public override Type CheckTypes()
        {
            //TODO base ctor call
            BaseArguments?.ForEach(x => x.CheckTypes());
            Body?.CheckTypes();

            return null;
        }
    }
}
