using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements;

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
                    return CreateMethodBody(parameters);
                default:
                    throw new Exception($"Unrecognized class name:{className}");
            }
        }

        private Node CreateMethodBody(List<List<Node>> parameters)
        {
            parameters.CheckLength(1);
            return new MethodBody
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
                case "BinaryExp":
                    return CreateBinaryExp(parameters);
                case "UnaryExp":
                    return CreateUnaryExp(parameters);
                default:
                    throw new Exception($"Unrecognized class name:{className}");
            }
        }

        private Node CreateMethod(List<Node> parameters)
        {
            parameters.CheckLength(5);
            return new Method
            {
                Visibility = (LexemeNode)parameters[0],
                Virtual_Override = (LexemeNode)parameters[1],
                ReturnType = (LexemeNode)parameters[2],
                Name = (LexemeNode)parameters[3],
                Params = (ParamList)parameters[4],
                Body = (MethodBody)parameters[5],
            };
        }


        private Node CreateClass(List<Node> parameters)
        {
            parameters.CheckLength(3);
            return new Class
            {
                Name = (LexemeNode)parameters[0],
                Extends = (LexemeNode)parameters[1],
                Body = (ClassBody)parameters[3],
            };
        }

        private Node CreateBinaryExp(List<Node> parameters)
        {
            parameters.CheckLength(3);
            return new BinaryExp
            {
                Left = (Expression)parameters[0],
                Operator = (LexemeNode)parameters[1],
                Right = (Expression)parameters[2]
            };
        }

        private Node CreateUnaryExp(List<Node> parameters)
        {
            parameters.CheckLength(2);
            return new UnaryExp
            {
                Operator = (LexemeNode)parameters[0],
                Right = (Expression)parameters[1]
            };
        }
    }
}
