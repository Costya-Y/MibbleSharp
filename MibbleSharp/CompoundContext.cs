﻿//
// CompoundContext.cs
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

namespace MibbleSharp
{
    /// <summary>
    /// A compound MIB context. This class attempts to resolve all symbols
    /// with either one of two MIB contexts, one of which will have
    /// priority.
    /// </summary>
    class CompoundContext : MibContext
    {

        private MibContext first;

        private MibContext second;

        ///
        /// <summary>Creates a new compound MIB context.</summary>
        ///    
        /// <param name="first">The primary MIB context</param>
        /// <param name="second">The secondary MIB context</param>
        ///
        public CompoundContext(MibContext first, MibContext second)
        {
            this.first = first;
            this.second = second;
        }

        ///
        /// <summary>
        /// Searches for a named MIB symbol. This method may search outside
        /// the normal (or strict) scope, thereby allowing a form of
        /// relaxed search. Note that the results from the normal and
        /// expanded search may not be identical, due to the context
        /// chaining and the same symbol name appearing in various
        /// contexts.
        /// </summary>
        /// <remark>
        /// NOTE: This is an internal method that should
        /// only be called by the MIB loader.
        /// </remark>
        ///
        /// <param name="name">The symbol name</param>
        /// <param name="expanded">The expanded scope flag</param>
        ///
        /// <returns>The MIBSymbol, or null if not found</returns>
        ///
        public MibSymbol FindSymbol(string name, bool expanded)
        {
            MibSymbol symbol;

            symbol = first.FindSymbol(name, expanded);
            if (symbol == null)
            {
                symbol = second.FindSymbol(name, expanded);
            }
            return symbol;
        }

        /// <summary>
        /// Get a string representation of the Compound Context
        /// </summary>
        /// <returns>A string representing the Compound Context</returns>
        public override string ToString()
        {
            return first.ToString() + ", " + second.ToString();
        }
    }

}
