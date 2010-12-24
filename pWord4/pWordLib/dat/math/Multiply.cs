﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pWordLib.dat;
using myPword;
using myPword.dat;
using System.Diagnostics;
using System.Drawing;

namespace pWordLib.dat.math
{
    public class Multiply : IOperate
    {
        private Icon symbol;

        public Multiply(Icon symbol)
        {
            this.symbol = symbol;
        }


        #region IOperate Members


        public pNode Operate(pNode _pNode)
        {
            // perform a summation on only child pNode elements
            // i.e.  this.Tag = total.ToString();
            decimal total = 0.0M;  // start off with 0
            int index = 0;
            foreach (pNode node in _pNode.Nodes)
            {
                node.PerformOperations();  // if there are no operations it will assume this is not an operator and treat it only as a value field
                decimal num = 0.0M;
                if (index++ == 0)
                {
                    // first time around get the total
                    Decimal.TryParse((String)node.Tag, out total);
                    continue;
                }
                
                // note: to make this a little more clear, if it is an operations field this current pNode has child nodes under it, it
                // will then process all child nodes under it based on whatever type of operaiton it is performing

                // attempt to convert to decimal and place it in num and perform the multiplication operation
                if (Decimal.TryParse((String)node.Tag, out num))
                {
                    total *= num; // perform the basic summation
                }
                else
                {
                    Debug.WriteLine("A Node failed to Sum");
                }

                //note: eventially I want to add advanced summation on (n^2+n)/2 with i=1 etc... but for now it just totallys up the values
            }
            _pNode.Tag = total.ToString();
            return _pNode;  // not yet implemented
        }
        public Icon Symbol
        {
            get { return symbol; }
        }

        #endregion

        
    }
}

