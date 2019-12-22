using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Binary;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Binary.Dot;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Unary;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements.If;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements.IO;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;
using ValueType = AbstractSyntaxTree_Implementation.Nodes.Types.ValueType;

namespace AbstractSyntaxTree_Implementation
{
    public class NodeFactory
    {
        public Node CreateNode(string className, List<List<Node>> parameters)
        {
            switch (className)
            {
                case "Program":
                    return CreateProgram(parameters);
                case "ClassBody":
                    return CreateClassBody(parameters);
                case "MethodBody":
                    return CreateBody(parameters);
                case "NodeList":
                    return CreateNodeList(parameters);
                default:
                    throw new Exception($"Unrecognized class name:{className}");
            }
        }

        private Node CreateNodeList(List<List<Node>> parameters)
        {
            parameters.CheckLength(1);
            var node = new NodeList
            {
                Nodes = parameters[0]
            };

            return node;
        }

        private Node CreateBody(List<List<Node>> parameters)
        {
            parameters.CheckLength(1);
            var node = new Body
            {
                Statements = parameters[0].Cast<Statement>().ToList()
            };

            node.AddChildren(node.Statements?.ToArray());

            return node;
        }

        private Node CreateClassBody(List<List<Node>> parameters)
        {
            parameters.CheckLength(1);
            var node = new ClassBody
            {
                Members = parameters[0].Cast<ClassMember>().ToList()
            };

            node.AddChildren(node.Members?.ToArray());

            return node;
        }

        private Node CreateProgram(List<List<Node>> parameters)
        {
            parameters.CheckLength(1);
            var node =  new Program
            {
                Classes = parameters[0].Cast<Class>().ToList()
            };

            node.AddChildren(node.Classes?.ToArray());

            return node;
        }

        public Node CreateNode(string className, List<Node> parameters)
        {
            switch (className)
            {
                case "Class":
                    return CreateClass(parameters);
                case "Method":
                    return CreateMethod(parameters);
                case "Constructor":
                    return CreateConstructor(parameters);
                case "Visibility":
                    return CreateTokenNode<Visibility>(parameters);

                case "ReferenceType":
                    return CreateTokenNode<ReferenceType>(parameters);
                case "ValueType":
                    return CreateTokenNode<ValueType>(parameters);

                case "LiteralExp":
                    return CreateTokenNode<LiteralExp>(parameters);
                case "VariableExp":
                    return CreateTokenNode<VariableExp>(parameters);
                case "LoopControl":
                    return CreateTokenNode<LoopControl>(parameters);
                case "LocalVariableDeclaration":
                    return CreateLocalVariableDeclaration(parameters);
                case "VariableDeclaration":
                    return CreateVariableDeclaration(parameters);
                case "Read":
                    return CreateRead(parameters);
                case "Write":
                    return CreateWrite(parameters);

                case "MemberExp":
                    return CreateMemberExp(parameters);
                case "ArithExp":
                    return CreateBinaryExp<ArithExp>(parameters);
                case "ComparisonExp":
                    return CreateBinaryExp<ComparisonExp>(parameters);
                case "EqualityExp":
                    return CreateBinaryExp<EqualityExp>(parameters);
                case "LogicExp":
                    return CreateBinaryExp<LogicExp>(parameters);

                case "UnaryArithExp":
                    return CreateUnaryExp<UnaryArithExp>(parameters);
                case "UnaryLogicExp":
                    return CreateUnaryExp<UnaryLogicExp>(parameters);

                case "ObjCreationExp":
                    return CreateObjCreationExp(parameters);
                case "Assign":
                    return CreateAssign(parameters);
                case "CallExp":
                    return CreateCallExp(parameters);
                case "If":
                    return CreateIf(parameters);
                case "Else":
                    return CreateElse(parameters);
                case "Loop":
                    return CreateLoop(parameters);
                case "ExpressionStatement":
                    return CreateExpressionStatement(parameters);
                case "Parameter":
                    return CreateParameter(parameters);
                case "Return":
                    return CreateReturn(parameters);
                case "Cast":
                    return CreateCast(parameters);

                default:
                    throw new Exception($"Unrecognized class name: {className}");
            }
        }

        private MemberExp CreateMemberExp(List<Node> parameters)
        {
            parameters.CheckLength(3);
            var right = parameters[2];
            if (right is CallExp)
                return CreateMemberExp<MemberCallExp>(parameters);
            if (right is VariableExp)
                return CreateMemberExp<MemberAccessExp>(parameters);
            throw new Exception("Right side of member expression can only be CallExp or VariableExp");
        }

        private MemberExp CreateMemberExp<T>(List<Node> parameters) where T : MemberExp, new ()
        {
            return CreateBinaryExp<T>(parameters);
        }

        private Cast CreateCast(List<Node> parameters)
        {
            parameters.CheckLength(2);
            var node = new Cast
            {
                Type = (Type)parameters[0],
                Expression = (Expression)parameters[1]
            };

            node.AddChildren(node.Type, node.Expression);

            return node;
        }

        private Return CreateReturn(List<Node> parameters)
        {
            parameters.CheckLength(1);
            var node = new Return
            {
                Expression = (Expression)parameters[0]
            };

            node.AddChildren(node.Expression);

            return node;
        }

        private Parameter CreateParameter(List<Node> parameters)
        {
            parameters.CheckLength(2);
            var node = new Parameter
            {
                Type = (Type)parameters[0],
                Name = (TokenNode)parameters[1]
            };

            node.AddChildren(node.Type, node.Name);

            return node;
        }

        private Constructor CreateConstructor(List<Node> parameters)
        {
            parameters.CheckLength(4);
            var node = new Constructor
            {
                Visibility = (Visibility)parameters[0],
                Parameters = ((NodeList)parameters[1])?.Nodes.Cast<Parameter>().ToList(),
                BaseArguments = ((NodeList)parameters[2])?.Nodes.Cast<Expression>().ToList(),
                Body = (Body)parameters[3]
            };

            node.AddChildren(node.Body, node.Visibility, node.Body);
            node.AddChildren(node.BaseArguments?.ToArray());
            node.AddChildren(node.Parameters?.ToArray());

            return node;
        }

        private ExpressionStatement CreateExpressionStatement(List<Node> parameters)
        {
            parameters.CheckLength(1);
            var node = new ExpressionStatement
            {
                Expression = (Expression)parameters[0]
            };

            node.AddChildren(node.Expression);

            return node;
        }

        private Loop CreateLoop(List<Node> parameters)
        {
            parameters.CheckLength(2);
            var node = new Loop
            {
                Condition = (Expression)parameters[0],
                Body = (Body)parameters[1]
            };

            node.AddChildren(node.Body, node.Condition);

            return node;
        }

        private Else CreateElse(List<Node> parameters)
        {
            parameters.CheckLength(2);

            if((parameters[0] == null) == (parameters[1] == null))
                throw new ArgumentException("invalid arguments for else");

            var node = new Else
            {
                If = (If)parameters[0],
                Body = (Body)parameters[1]
            };

            node.AddChildren(node.If, node.Body);

            return node;
        }

        private If CreateIf(List<Node> parameters)
        {
            parameters.CheckLength(4);
            var node = new If
            {
                KwIf = (TokenNode)parameters[0],
                Condition = (Expression)parameters[1],
                Body = (Body)parameters[2],
                Else = (Else)parameters[3]
            };

            node.AddChildren(node.Condition, node.Body, node.Body, node.Else);

            return node;
        }

        private CallExp CreateCallExp(List<Node> parameters)
        {
            parameters.CheckLength(2);
            var node = new CallExp
            {
                MethodName = (TokenNode)parameters[0],
                Arguments = ((NodeList)parameters[1])?.Nodes.Cast<Expression>().ToList()
            };

            node.AddChildren(node.MethodName);
            node.AddChildren(node.Arguments?.ToArray());

            return node;
        }

        private Assign CreateAssign(List<Node> parameters)
        {
            parameters.CheckLength(2);
            var node = new Assign
            {
                Variable = (TokenNode)parameters[0],
                Expression = (Expression)parameters[1]
            };

            node.AddChildren(node.Variable, node.Expression);

            return node;
        }

        private newObjectExp CreateObjCreationExp(List<Node> parameters)
        {
            parameters.CheckLength(2);
            var node = new newObjectExp
            {
                Type = (ReferenceType)parameters[0],
                Arguments = ((NodeList)parameters[1])?.Nodes.Cast<Expression>().ToList()
            };

            node.AddChildren(node.Type);
            node.AddChildren(node.Arguments?.ToArray());

            return node;
        }

        private Write CreateWrite(List<Node> parameters)
        {
            parameters.CheckLength(1);
            var node = new Write
            {
                Arguments = ((NodeList)parameters[0]).Nodes.Cast<Expression>().ToList()
            };

            node.AddChildren(node.Arguments?.ToArray());

            return node;
        }

        private Read CreateRead(List<Node> parameters)
        {
            parameters.CheckLength(1);
            var node = new Read
            {
                Variables = ((NodeList)parameters[0]).Nodes.Cast<TokenNode>().ToList()
            };

            node.AddChildren(node.Variables?.ToArray());

            return node;
        }

        private VariableDeclaration CreateVariableDeclaration(List<Node> parameters)
        {
            parameters.CheckLength(4);
            var node = new VariableDeclaration
            {
                Visibility = (Visibility)parameters[0],
                Type = (Type)parameters[1],
                Name = (TokenNode)parameters[2],
                Expression = (Expression)parameters[3]
            };

            node.AddChildren(node.Visibility, node.Type, node.Name, node.Expression);

            return node;
        }

        private LocalVariableDeclaration CreateLocalVariableDeclaration(List<Node> parameters)
        {
            parameters.CheckLength(3);
            var node = new LocalVariableDeclaration
            {
                Type = (Type)parameters[0],
                Name = (TokenNode)parameters[1],
                Expression = (Expression)parameters[2]
            };

            node.AddChildren(node.Type, node.Name, node.Expression);

            return node;
        }

        private T CreateTokenNode<T>(List<Node> parameters) where T : Node, ITokenNode, new()
        {
            parameters.CheckLength(1);
            var token = (TokenNode) parameters[0];
            return new T
            {
                TokenType = token.TokenType,
                Value = token.Value,
                Line = token.Line
            };
        }

        private Node CreateMethod(List<Node> parameters)
        {
            parameters.CheckLength(6);
            var node = new Method
            {
                Visibility = (Visibility) parameters[0],
                Virtual_Override = (TokenNode) parameters[1],
                ReturnType = (Type) parameters[2],
                Name = (TokenNode) parameters[3],
                Parameters = ((NodeList) parameters[4])?.Nodes.Cast<Parameter>().ToList(),
                Body = (Body) parameters[5],
            };

            node.AddChildren(node.Visibility, node.Virtual_Override, node.ReturnType, node.Name, node.Body);
            node.AddChildren(node.Parameters?.ToArray());

            return node;
        }


        private Node CreateClass(List<Node> parameters)
        {
            parameters.CheckLength(3);
            var node = new Class
            {
                Name = (TokenNode)parameters[0],
                Extends = (ReferenceType)parameters[1],
                Body = (ClassBody)parameters[2],
            };

            node.AddChildren(node.Name, node.Extends, node.Body);

            return node;
        }

        private T CreateBinaryExp<T>(List<Node> parameters) where T : BinaryExp, new()
        {
            parameters.CheckLength(3);
            var node = new T
            {
                Left = (Expression)parameters[0],
                Operator = (TokenNode)parameters[1],
                Right = (Expression)parameters[2]
            };

            node.AddChildren(node.Left, node.Operator, node.Right);

            return node;
        }

        private T CreateUnaryExp<T>(List<Node> parameters) where T : UnaryExp, new()
        {
            parameters.CheckLength(2);
            var node = new T
            {
                Operator = (TokenNode)parameters[0],
                Expression = (Expression)parameters[1]
            };

            node.AddChildren(node.Operator, node.Expression);

            return node;
        }
    }
}
