using System;

namespace FundamentalsAggregator
{
    public class TickerSymbol : IEquatable<TickerSymbol>
    {
        public TickerSymbol(string symbol, Exchange exchange)
        {
            if (symbol == null) throw new ArgumentNullException("symbol");
            Symbol = symbol.ToUpper();
            Exchange = exchange;
        }

        public string Symbol { get; private set; }
        public Exchange Exchange { get; private set; }

        public override string ToString()
        {
            return String.Format("{0} ({1})", Symbol, Exchange.ToString().ToUpper());
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(TickerSymbol other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Symbol, Symbol) && Equals(other.Exchange, Exchange);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (TickerSymbol)) return false;
            return Equals((TickerSymbol) obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Symbol != null ? Symbol.GetHashCode() : 0)*397) ^ Exchange.GetHashCode();
            }
        }
    }
}