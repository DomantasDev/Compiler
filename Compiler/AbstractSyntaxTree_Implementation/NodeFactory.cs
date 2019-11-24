using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Binary;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Unary;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements.If;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements.IO;
using Type = AbstractSyntaxTree_Implementation.Nodes.Type;

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
            return new NodeList
            {
                Nodes = parameters[0]
            };
        }

        private Node CreateBody(List<List<Node>> parameters)
        {
            parameters.CheckLength(1);
            return new Body
            {
                Statements = parameters[0].Cast<Statement>().ToList()
            };
        }

        private Node CreateClassBody(List<List<Node>> parameters)
        {
            parameters.CheckLength(1);
            return new ClassBody
            {
                Members = parameters[0].Cast<ClassMember>().ToList()
            };
        }

        private Node CreateProgram(List<List<Node>> parameters)
        {
            parameters.CheckLength(1);
            return new Program
            {
                CLasses = parameters[0].Cast<Class>().ToList()
            };
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
                case "Type":
                    return CreateTokenNode<Type>(parameters);
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
                case "BinaryExp":
                    return CreateBinaryExp<BinaryExp>(parameters);
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

        private Cast CreateCast(List<Node> parameters)
        {
            parameters.CheckLength(2);
            return new Cast
            {
                Type = (Type)parameters[0],
                Expression = (Expression)parameters[1]
            };
        }

        private Return CreateReturn(List<Node> parameters)
        {
            parameters.CheckLength(1);
            return new Return
            {
                Expression = (Expression)parameters[0]
            };
        }

        private Parameter CreateParameter(List<Node> parameters)
        {
            parameters.CheckLength(2);
            return new Parameter
            {
                Type = (Type)parameters[0],
                Name = (TokenNode)parameters[1]
            };
        }

        private Constructor CreateConstructor(List<Node> parameters)
        {
            parameters.CheckLength(4);
            return new Constructor
            {
                Visibility = (Visibility)parameters[0],
                Parameters = ((NodeList)parameters[1])?.Nodes.Cast<Parameter>().ToList(),
                BaseArguments = ((NodeList)parameters[2])?.Nodes.Cast<Expression>().ToList(),
                Body = (Body)parameters[3]
            };
        }

        private ExpressionStatement CreateExpressionStatement(List<Node> parameters)
        {
            parameters.CheckLength(1);
            return new ExpressionStatement
            {
                Expression = (Expression)parameters[0]
            };
        }

        private Loop CreateLoop(List<Node> parameters)
        {
            parameters.CheckLength(2);
            return new Loop
            {
                Condition = (Expression)parameters[0],
                Body = (Body)parameters[1]
            };
        }

        private Else CreateElse(List<Node> parameters)
        {
            parameters.CheckLength(2);

            if((parameters[0] == null) == (parameters[1] == null))
                throw new ArgumentException("invalid arguments for else");

            return new Else
            {
                If = (If)parameters[0],
                Body = (Body)parameters[1]
            };
        }

        private If CreateIf(List<Node> parameters)
        {
            parameters.CheckLength(3);
            return new If
            {
                Condition = (Expression)parameters[0],
                Body = (Body)parameters[1],
                Else = (Else)parameters[2]
            };
        }

        private CallExp CreateCallExp(List<Node> parameters)
        {
            parameters.CheckLength(2);
            return new CallExp
            {
                MethodName = (TokenNode)parameters[0],
                Arguments = ((NodeList)parameters[1])?.Nodes.Cast<Expression>().ToList()
            };
        }

        private Assign CreateAssign(List<Node> parameters)
        {
            parameters.CheckLength(2);
            return new Assign
            {
                VariableName = (TokenNode)parameters[0],
                Expression = (Expression)parameters[1]
            };
        }

        private ObjCreationExp CreateObjCreationExp(List<Node> parameters)
        {
            parameters.CheckLength(2);
            return new ObjCreationExp
            {
                Type = (Type)parameters[0],
                Arguments = ((NodeList)parameters[1])?.Nodes.Cast<Expression>().ToList()
            };
        }

        private Write CreateWrite(List<Node> parameters)
        {
            parameters.CheckLength(1);
            return new Write
            {
                Arguments = ((NodeList)parameters[0]).Nodes.Cast<Expression>().ToList()
            };
        }

        private Read CreateRead(List<Node> parameters)
        {
            parameters.CheckLength(1);
            return new Read
            {
                Variables = ((NodeList)parameters[0]).Nodes.Cast<TokenNode>().ToList()
            };
        }

        private VariableDeclaration CreateVariableDeclaration(List<Node> parameters)
        {
            parameters.CheckLength(4);
            return new VariableDeclaration
            {
                Visibility = (Visibility)parameters[0],
                Type = (Type)parameters[1],
                Name = (TokenNode)parameters[2],
                Expression = (Expression)parameters[3]
            };
        }

        private LocalVariableDeclaration CreateLocalVariableDeclaration(List<Node> parameters)
        {
            parameters.CheckLength(3);
            return new LocalVariableDeclaration
            {
                Type = (Type)parameters[0],
                Name = (TokenNode)parameters[1],
                Expression = (Expression)parameters[2]
            };
        }

        private T CreateTokenNode<T>(List<Node> parameters) where T : Node, ITokenNode, new()
        {
            parameters.CheckLength(1);
            return new T
            {
                Token = ((TokenNode)parameters[0]).Token
            };
        }

        private Node CreateMethod(List<Node> parameters)
        {
            parameters.CheckLength(6);
            return new Method
            {
                Visibility = (Visibility) parameters[0],
                Virtual_Override = (TokenNode) parameters[1],
                ReturnType = (Type) parameters[2],
                Name = (TokenNode) parameters[3],
                Parameters = ((NodeList) parameters[4])?.Nodes.Cast<Parameter>().ToList(),
                Body = (Body) parameters[5],
            };
        }


        private Node CreateClass(List<Node> parameters)
        {
            parameters.CheckLength(3);
            return new Class
            {
                Name = (TokenNode)parameters[0],
                Extends = (TokenNode)parameters[1],
                Body = (ClassBody)parameters[2],
            };
        }

        private T CreateBinaryExp<T>(List<Node> parameters) where T : BinaryExp, new()
        {
            parameters.CheckLength(3);
            return new T
            {
                Left = (Expression)parameters[0],
                Operator = (TokenNode)parameters[1],
                Right = (Expression)parameters[2]
            };
        }

        private T CreateUnaryExp<T>(List<Node> parameters) where T : UnaryExp, new()
        {
            parameters.CheckLength(2);
            return new T
            {
                Operator = (TokenNode)parameters[0],
                Expression = (Expression)parameters[1]
            };
        }
    }
}
