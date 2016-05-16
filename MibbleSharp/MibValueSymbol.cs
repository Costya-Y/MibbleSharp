﻿//
// MibValueSymbol.cs
// 
// This work is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published
// by the Free Software Foundation; either version 2 of the License,
// or (at your option) any later version.
//
// This work is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307
// USA
// 
// Original Java code Copyright (c) 2004-2016 Per Cederberg. All
// rights reserved.
// C# conversion Copyright (c) 2016 Jeremy Gibbons. All rights reserved
//

using System.Text;
using MibbleSharp.Value;
using MibbleSharp.Snmp;
using MibbleSharp.Type;

namespace MibbleSharp
{
    public class MibValueSymbol : MibSymbol
    {

        /**
         * A MIB value symbol. This class holds information relevant to a MIB
         * value assignment, i.e. a type and a value. Normally the value is
         * an object identifier.
         *
         * @author   Per Cederberg, <per at percederberg dot net>
         * @version  2.8
         * @since    2.0
         */

        /**
         * The symbol type.
         */
        private MibType type;

        /**
         * The symbol value.
         */
        private MibValue value;

        /**
         * Creates a new value symbol.<p>
         *
         * <strong>NOTE:</strong> This is an internal constructor that
         * should only be called by the MIB loader.
         *
         * @param location       the symbol location
         * @param mib            the symbol MIB file
         * @param name           the symbol name
         * @param type           the symbol type
         * @param value          the symbol value
         *
         * @since 2.2
         */
        public MibValueSymbol(FileLocation location,
                              Mib mib,
                              string name,
                              MibType type,
                              MibValue value) : base(location, mib, name)
        {

            this.type = type;
            this.value = value;
        }

        /**
         * Initializes the MIB symbol. This will remove all levels of
         * indirection present, such as references to types or values. No
         * information is lost by this operation. This method may modify
         * this object as a side-effect.<p>
         *
         * <strong>NOTE:</strong> This is an internal method that should
         * only be called by the MIB loader.
         *
         * @param log            the MIB loader log
         *
         * @throws MibException if an error was encountered during the
         *             initialization
         */
        public override void Initialize(MibLoaderLog log) //throws MibException
        {
            ObjectIdentifierValue oid;

        if (type != null) {
                try
                {
                    type = type.Initialize(this, log);
                }
                catch (MibException e)
                {
                    log.AddError(e.Location, e.Message);
                    type = null;
                }
            }
        if (value != null) {
                try
                {
                    value = value.Initialize(log, type);
                }
                catch (MibException e)
                {
                    log.AddError(e.Location, e.Message);
                    value = null;
                }
            }
        if (type != null && value != null && !type.IsCompatible(value)) {
                log.AddError(getLocation(),
                             "value is not compatible with type");
            }
        if (value is ObjectIdentifierValue) {
                oid = (ObjectIdentifierValue)value;
                if (oid.getSymbol() == null)
                {
                    oid.setSymbol(this);
                }
            }
        }

        /**
         * Clears and prepares this MIB symbol for garbage collection.
         * This method will recursively clear any associated types or
         * values, making sure that no data structures references this
         * symbol.
         */
        public override void Clear()
        {
            type = null;
            if (value != null)
            {
                value.Clear();
            }
            value = null;
        }

        /**
         * Checks if this symbol corresponds to a scalar. A symbol is
         * considered a scalar if it has an SnmpObjectType type and does
         * not represent or reside within a table.
         *
         * @return true if this symbol is a scalar, or
         *         false otherwise
         *
         * @see #isTable()
         * @see #isTableRow()
         * @see #isTableColumn()
         * @see net.percederberg.mibble.snmp.SnmpObjectType
         *
         * @since 2.5
         */
        public bool isScalar()
        {
            return type is SnmpObjectType
            && !isTable()
            && !isTableRow()
            && !isTableColumn();
        }

        /**
         * Checks if this symbol corresponds to a table. A symbol is
         * considered a table if it has an SnmpObjectType type with
         * SEQUENCE OF syntax.
         *
         * @return true if this symbol is a table, or
         *         false otherwise
         *
         * @see #isScalar()
         * @see #isTableRow()
         * @see #isTableColumn()
         * @see net.percederberg.mibble.snmp.SnmpObjectType
         *
         * @since 2.5
         */
        public bool isTable()
        {
            MibType syntax;

            if (type is SnmpObjectType) {
                syntax = ((SnmpObjectType)type).getSyntax();
                return syntax is SequenceOfType;
            } else {
                return false;
            }
        }

        /**
         * Checks if this symbol corresponds to a table row (or entry). A
         * symbol is considered a table row if it has an SnmpObjectType
         * type with SEQUENCE syntax.
         *
         * @return true if this symbol is a table row, or
         *         false otherwise
         *
         * @see #isScalar()
         * @see #isTable()
         * @see #isTableColumn()
         * @see net.percederberg.mibble.snmp.SnmpObjectType
         *
         * @since 2.5
         */
        public bool isTableRow()
        {
            MibType syntax;

            if (type is SnmpObjectType) {
                syntax = ((SnmpObjectType)type).getSyntax();
                return syntax is SequenceType;
            } else {
                return false;
            }
        }

        /**
         * Checks if this symbol corresponds to a table column. A symbol
         * is considered a table column if it has an SnmpObjectType type
         * and a parent symbol that is a table row.
         *
         * @return true if this symbol is a table column, or
         *         false otherwise
         *
         * @see #isScalar()
         * @see #isTable()
         * @see #isTableRow()
         * @see net.percederberg.mibble.snmp.SnmpObjectType
         *
         * @since 2.5
         */
        public bool isTableColumn()
        {
            MibValueSymbol parent = getParent();

            return type is SnmpObjectType
            && parent != null
            && parent.isTableRow();
        }

        /**
         * Returns the symbol type.
         *
         * @return the symbol type
         */
        public MibType getType()
        {
            return type;
        }

        /**
         * Returns the symbol value.
         *
         * @return the symbol value
         */
        public MibValue getValue()
        {
            return value;
        }

        /**
         * Returns the parent symbol in the OID tree. This is a
         * convenience method for value symbols that have object
         * identifier values. 
         *
         * @return the parent symbol in the OID tree, or
         *         null for none or if not applicable
         *
         * @see net.percederberg.mibble.value.ObjectIdentifierValue
         *
         * @since 2.5
         */
        public MibValueSymbol getParent()
        {
            ObjectIdentifierValue oid;

            if (value is ObjectIdentifierValue) {
                oid = ((ObjectIdentifierValue)value).getParent();
                if (oid != null)
                {
                    return oid.getSymbol();
                }
            }
            return null;
        }

        /**
         * Returns the number of child symbols in the OID tree. This is a
         * convenience method for value symbols that have object
         * identifier values. 
         *
         * @return the number of child symbols in the OID tree, or
         *         zero (0) if not applicable
         *
         * @see net.percederberg.mibble.value.ObjectIdentifierValue
         *
         * @since 2.6
         */
        public int getChildCount()
        {
            ObjectIdentifierValue oiv = value as ObjectIdentifierValue;
            if (value == null)
                return 0;
            
            return oiv.getChildCount();
        }

        /**
         * Returns a specific child symbol in the OID tree. This is a
         * convenience method for value symbols that have object
         * identifier values. 
         *
         * @param index          the child position, 0 <= index < count
         *
         * @return the child symbol in the OID tree, or
         *         null if not found or not applicable
         *
         * @see net.percederberg.mibble.value.ObjectIdentifierValue
         *
         * @since 2.6
         */
        public MibValueSymbol getChild(int index)
        {
            ObjectIdentifierValue child;

            ObjectIdentifierValue oiv = value as ObjectIdentifierValue;

            if (oiv == null)
                return null;

            child = oiv.getChild(index);
            if (child != null)
                return child.getSymbol();

            return null;
        }

        /**
         * Returns all child symbols in the OID tree. This is a
         * convenience method for value symbols that have object
         * identifier values. 
         *
         * @return the array of child symbols in the OID tree, or
         *         an empty array if not applicable
         *
         * @see net.percederberg.mibble.value.ObjectIdentifierValue
         *
         * @since 2.6
         */
        public MibValueSymbol[] getChildren()
        {
            MibValueSymbol[] children;

            ObjectIdentifierValue oid = value as ObjectIdentifierValue;

            if (value == null)
                return new MibValueSymbol[0];

            children = new MibValueSymbol[oid.getChildCount()];

            for (int i = 0; i < oid.getChildCount(); i++)
            {
                children[i] = oid.getChild(i).getSymbol();
            }

            return children;
        }

        /**
         * Returns a string representation of this object.
         *
         * @return a string representation of this object
         */
        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();

            buffer.Append("VALUE ");
            buffer.Append(getName());
            buffer.Append(" ");
            buffer.Append(getType());
            buffer.Append("\n    ::= ");
            buffer.Append(getValue());
            return buffer.ToString();
        }
    }
}

