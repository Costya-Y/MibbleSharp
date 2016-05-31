// <copyright file="Analyzer.cs" company="None">
//    <para>
//    This program is free software: you can redistribute it and/or
//    modify it under the terms of the BSD license.</para>
//    <para>
//    This work is distributed in the hope that it will be useful, but
//    WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.</para>
//    <para>
//    See the LICENSE.txt file for more details.</para>
//    Original code as generated by Grammatica 1.6 Copyright (c) 
//    2003-2015 Per Cederberg. All rights reserved.
//    Updates Copyright (c) 2016 Jeremy Gibbons. All rights reserved
// </copyright>

namespace PerCederberg.Grammatica.Runtime
{
    using System.Collections;

    /// <summary><para>
    /// A parse tree analyzer. This class provides callback methods that
    /// may be used either during parsing, or for a parse tree traversal.
    /// This class should be sub-classed to provide adequate handling of the
    /// parse tree nodes.
    /// </para><para>
    /// The general contract for the analyzer class does not guarantee a
    /// strict call order for the callback methods. Depending on the type
    /// of parser, the enter() and exit() methods for production nodes can
    /// be called either in a top-down or a bottom-up fashion. The only
    /// guarantee provided by this API, is that the calls for any given
    /// node will always be in the order enter(), child(), and exit(). If
    /// various child() calls are made, they will be made from left to
    /// right as child nodes are added (to the right).
    /// </para></summary>
    public class Analyzer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Analyzer"/> class.
        /// </summary>
        public Analyzer()
        {
        }

        /// <summary>
        /// Resets this analyzer when the parser is reset for another
        /// input stream. The default implementation of this method does
        /// nothing.
        /// </summary>         
        public virtual void Reset()
        {
            // Default implementation does nothing
        }

        /// <summary>
        /// Analyzes a parse tree node by traversing all it's child nodes.
        /// The tree traversal is depth-first, and the appropriate
        /// callback methods will be called. If the node is a production
        /// node, a new production node will be created and children will
        /// be added by recursively processing the children of the
        /// specified production node. This method is used to process a
        /// parse tree after creation.
        /// </summary>
        /// <param name="node">The parse tree node to process</param>
        /// <returns>The resulting parse tree node</returns>
        /// <exception cref="ParserLogException">
        /// If un recoverable errors occurred during the analysis
        /// </exception>         
        public Node Analyze(Node node)
        {
            ParserLogException log = new ParserLogException();

            node = this.Analyze(node, log);

            if (log.Count > 0)
            {
                throw log;
            }

            return node;
        }

        /// <summary>
        /// Factory method to create a new production node. This method
        /// can be overridden to provide other production implementations
        /// than the default one.
        /// </summary>
        /// <param name="pattern">The production pattern</param>
        /// <returns>The new production node</returns>
        public virtual Production NewProduction(ProductionPattern pattern)
        {
            return new Production(pattern);
        }

        /// <summary>
        /// Called when entering a parse tree node. By default this method
        /// does nothing. A subclass can override this method to handle
        /// each node separately.
        /// </summary>
        /// <param name="node">The node being entered</param>
        /// <exception cref="ParseException">
        /// If the node analysis discovered errors
        /// </exception>
        public virtual void Enter(Node node)
        {
        }

        /// <summary>
        /// Called when exiting a parse tree node. By default this method
        /// returns the node. A subclass can override this method to handle
        /// each node separately. If no parse tree should be created, this
        /// method should return null.
        /// </summary>
        /// <param name="node">The node being exited</param>
        /// <returns>The node to add to the parse tree</returns>
        /// <returns>
        /// the node to add to the parse tree, or null if none was created
        ///  null if no parse tree should be created
        ///  </returns>
        /// <exception cref="ParseException">
        /// If errors occurred during the node analysis.
        /// </exception>
        public virtual Node Exit(Node node)
        {
            return node;
        }

        /// <summary>
        /// Called when adding a child to a parse tree node. By default
        /// this method adds the child to the production node. A subclass
        /// can override this method to handle each node separately. Note
        /// that the child node may be null if the corresponding exit()
        /// method returned null.
        /// </summary>
        /// <param name="node">The parent node</param>
        /// <param name="child">The child node, or null</param>
        /// <exception cref="ParseException">
        /// If errors occurred during node analysis
        /// </exception>
        public virtual void Child(Production node, Node child)
        {
            node.AddChild(child);
        }

        /// <summary>
        /// Returns a child at the specified position. If either the node
        /// or the child node is null, this method will throw a parse
        /// exception with the internal error type.
        /// </summary>
        /// <param name="node">The parent node</param>
        /// <param name="pos">The child position</param>
        /// <returns>The child node</returns>
        /// <exception cref="ParseException">
        /// If either the node or the child node was null
        /// </exception> 
        protected Node GetChildAt(Node node, int pos)
        {
            Node child;

            if (node == null)
            {
                throw new ParseException(
                    ParseException.ErrorType.Internal,
                    "attempt to read 'null' parse tree node",
                    -1,
                    -1);
            }

            child = node[pos];

            if (child == null)
            {
                string msg = "node '" + node.Name + "' has no child at " +
                    "position " + pos;
                throw new ParseException(
                    ParseException.ErrorType.Internal,
                    msg,
                    node.StartLine,
                    node.StartColumn);
            }

            return child;
        }

        /// <summary>
        /// Returns the first child with the specified id. If the node is
        /// null, or no child with the specified id could be found, this
        /// method will throw a parse exception with the internal error
        /// type.
        /// </summary>
        /// <param name="node">The parent node</param>
        /// <param name="id">The child node id</param>
        /// <returns>The child node</returns>
        /// <exception cref="ParseException">
        /// If the node was null or a child node couldn't be found
        /// </exception>
        protected Node GetChildWithId(Node node, int id)
        {
            Node child;

            if (node == null)
            {
                throw new ParseException(
                    ParseException.ErrorType.Internal,
                    "attempt to read 'null' parse tree node",
                    -1,
                    -1);
            }

            for (int i = 0; i < node.ChildCount; i++)
            {
                child = node[i];
                if (child != null && child.Id == id)
                {
                    return child;
                }
            }

            throw new ParseException(
                ParseException.ErrorType.Internal,
                "node '" + node.Name + "' has no child with id " + id,
                node.StartLine,
                node.StartColumn);
        }

        /// <summary>
        /// Returns the node value at the specified position. If either
        /// the node or the value is null, this method will throw a parse
        /// exception with the internal error type.
        /// </summary>
        /// <param name="node">The parse tree node</param>
        /// <param name="pos">The child position</param>
        /// <returns>The value object</returns>
        /// <exception cref="ParseException">
        /// If either the node or the value was null
        /// </exception>
        protected object GetValue(Node node, int pos)
        {
            object value;

            if (node == null)
            {
                throw new ParseException(
                    ParseException.ErrorType.Internal,
                    "attempt to read 'null' parse tree node",
                    -1,
                    -1);
            }

            value = node.Values[pos];

            if (value == null)
            {
                string msg = "node '" + node.Name + "' has no value at " +
                    "position " + pos;
                throw new ParseException(
                    ParseException.ErrorType.Internal,
                    msg,
                    node.StartLine,
                    node.StartColumn);
            }

            return value;
        }

        /// <summary>
        /// Returns the node integer value at the specified position. If
        /// either the node is null, or the value is not an instance of
        /// the Integer class, this method will throw a parse exception
        /// with the internal error type.
        /// </summary>
        /// <param name="node">The parse tree node</param>
        /// <param name="pos">The child position</param>
        /// <returns>The value object</returns>
        /// <exception cref="ParseException">
        /// If the node was null, or the value was not an integer
        /// </exception>
        protected int GetIntValue(Node node, int pos)
        {
            object value;

            value = this.GetValue(node, pos);
            if (value is int)
            {
                return (int)value;
            }
            else
            {
                string msg = "node '" + node.Name + "' has no integer value " +
                    "at position " + pos;
                throw new ParseException(
                    ParseException.ErrorType.Internal,
                    msg,
                    node.StartLine,
                    node.StartColumn);
            }
        }

        /// <summary>
        /// Returns the node string value at the specified position. If
        /// either the node is null, or the value is not an instance of
        /// the String class, this method will throw a parse exception
        /// with the internal error type.
        /// </summary>
        /// <param name="node">The parse tree node</param>
        /// <param name="pos">The child position</param>
        /// <returns>The value object</returns>
        /// <exception cref="ParseException">
        /// If either the node was null, or the value was not a string
        /// </exception>
        protected string GetStringValue(Node node, int pos)
        {
            object value;

            value = this.GetValue(node, pos);

            if (value is string)
            {
                return (string)value;
            }
            else
            {
                string msg = "node '" + node.Name + "' has no string value " +
                    "at position " + pos;
                throw new ParseException(
                    ParseException.ErrorType.Internal,
                    msg,
                    node.StartLine,
                    node.StartColumn);
            }
        }

        /// <summary>
        /// Returns all the node values for all child nodes.
        /// </summary>
        /// <param name="node">The parse tree node</param>
        /// <returns>A list of all the child node values</returns>
        protected ArrayList GetChildValues(Node node)
        {
            ArrayList result = new ArrayList();
            Node child;
            ArrayList values;

            for (int i = 0; i < node.ChildCount; i++)
            {
                child = node[i];
                values = child.Values;
                if (values != null)
                {
                    result.AddRange(values);
                }
            }

            return result;
        }
        
        /// <summary>
        /// Analyzes a parse tree node by traversing all it's child nodes.
        /// The tree traversal is depth-first, and the appropriate
        /// callback methods will be called. If the node is a production
        /// node, a new production node will be created and children will
        /// be added by recursively processing the children of the
        /// specified production node. This method is used to process a
        /// parse tree after creation.
        /// </summary>
        /// <param name="node">The parse tree node to process</param>
        /// <param name="log">The parser error log</param>
        /// <returns>The resulting parse tree node</returns>         
        private Node Analyze(Node node, ParserLogException log)
        {
            Production prod;
            int errorCount;

            errorCount = log.Count;
            if (node is Production)
            {
                prod = (Production)node;
                prod = this.NewProduction(prod.Pattern);

                try
                {
                    this.Enter(prod);
                }
                catch (ParseException e)
                {
                    log.AddError(e);
                }

                for (int i = 0; i < node.ChildCount; i++)
                {
                    try
                    {
                        this.Child(prod, this.Analyze(node[i], log));
                    }
                    catch (ParseException e)
                    {
                        log.AddError(e);
                    }
                }

                try
                {
                    return this.Exit(prod);
                }
                catch (ParseException e)
                {
                    if (errorCount == log.Count)
                    {
                        log.AddError(e);
                    }
                }
            }
            else
            {
                node.Values.Clear();
                try
                {
                    this.Enter(node);
                }
                catch (ParseException e)
                {
                    log.AddError(e);
                }

                try
                {
                    return this.Exit(node);
                }
                catch (ParseException e)
                {
                    if (errorCount == log.Count)
                    {
                        log.AddError(e);
                    }
                }
            }

            return null;
        }
    }
}
