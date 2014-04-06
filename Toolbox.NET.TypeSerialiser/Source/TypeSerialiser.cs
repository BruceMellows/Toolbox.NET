// Copyright (c) 2014, Bruce Mellows
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
// 1. This file must retain the above copyright notice, this list of conditions
//    and the following disclaimer.
// 2. Redistributions of source code must retain the above copyright notice, this
//    list of conditions and the following disclaimer.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

namespace Toolbox.NET
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class TypeSerialiser : IFormattable
    {
        #region Constants

        public const string TransportableFormat = "XFER";
        public const string CSharpFormat = "CS";

        #endregion Constants

        #region Private Static

        private const char GenericBeginChar = '[';
        private const char GenericSeperatorChar = ',';
        private const char GenericEndChar = ']';
        private static string GenericSeperatorString = new string(GenericSeperatorChar, 1);
        private static string GenericEndString = new string(GenericEndChar, 1);

        private static Regex LabelRegex = new Regex(@"[_a-zA-Z][_a-zA-Z0-9]*(\.[_a-zA-Z][_a-zA-Z0-9]*)*", RegexOptions.Compiled);

        private static object cacheLock = new object();
        private static Dictionary<Type, TypeSerialiser> cachedTypeSerialisers = new Dictionary<Type, TypeSerialiser>();
        private static Dictionary<string, TypeSerialiser> cachedStringSerialisers = new Dictionary<string, TypeSerialiser>();

        #endregion Private Static

        #region Private Fields

        private string transportableString;
        private List<string> tokens;

        #endregion Private Fields

        #region Constructors

        public TypeSerialiser(string transportableString)
        {
            var cached = GetCached(this.transportableString = transportableString);
            if (cached != null)
            {
                this.Type = cached.Type;
                this.tokens = cached.tokens;
            }
            else
            {
                this.Type = FromTokenised(Tokenise(this.transportableString));
                this.UpdateCache();
            }
        }

        public TypeSerialiser(System.Type type)
        {
            var cached = GetCached(this.Type = type);
            if (cached != null)
            {
                this.Type = cached.Type;
                this.tokens = cached.tokens;
            }
            else
            {
                this.tokens = Tokenise(this.transportableString = ToTransportableString(this.Type, GenericBeginChar, GenericSeperatorChar, GenericEndChar));
                this.UpdateCache();
            }
        }

        #endregion Constructors

        #region Public Properties

        public System.Type Type { get; private set; }

        public bool ContainsGenericParameters
        {
            get
            {
                return this.Type.ContainsGenericParameters;
            }
        }

        public Type GetGenericType
        {
            get
            {
                if (!this.Type.ContainsGenericParameters)
                {
                    throw new InvalidOperationException();
                }

                var lookupName = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}`{1}", this.tokens[0], this.tokens.Count - 3);
                return System.Type.GetType(lookupName);
            }
        }

        public IEnumerable<TypeSerialiser> GetGenericArguments
        {
            get
            {
                if (!this.Type.ContainsGenericParameters)
                {
                    throw new InvalidOperationException();
                }

                return this.Type.GetGenericArguments().Select(x => new TypeSerialiser(x));
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return this.ToString(TransportableFormat);
        }

        public string ToString(string format)
        {
            return this.ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            format = !String.IsNullOrEmpty(format) ? format : TransportableFormat;
            formatProvider = formatProvider ?? CultureInfo.CurrentCulture;

            switch (format.ToUpperInvariant())
            {
                case TransportableFormat:
                    return this.transportableString;
                case CSharpFormat:
                    return ToTransportableString(this.Type, '<', ',', '>');
            }

            throw new FormatException(String.Format("The {0} format string is not supported.", format));
        }

        #endregion Public Methods

        #region Private Methods

        private void UpdateCache()
        {
            lock (cacheLock)
            {
                if (!cachedStringSerialisers.ContainsKey(this.transportableString))
                {
                    this.Type = FromTokenised(Tokenise(this.transportableString));
                    cachedStringSerialisers[this.transportableString] = this;
                }
                else
                {
                    var cached = cachedStringSerialisers[this.transportableString];
                    this.Type = cached.Type;
                }
            }
        }

        private static string ToTransportableString(System.Type type, char beginChar, char seperatorChar, char endChar)
        {
            var basicName = type.FullName;

            if (!type.IsGenericType)
                return basicName;

            return basicName.Substring(0, basicName.IndexOf('`'))
                + beginChar
                + string.Join(new string(seperatorChar, 1), type.GetGenericArguments().Select(x => ToTransportableString(x, beginChar, seperatorChar, endChar)))
                + endChar;
        }

        private static System.Type FromTokenised(List<string> tokens)
        {
            if (tokens == null)
                throw new System.ArgumentException();

            // FIXME - should validate tokens (grammar)

            int index = 0;
            return CreateType(tokens, ref index);
        }

        private static System.Type CreateType(List<string> tokens, ref int index)
        {
            var token = tokens[tokens.Count - ++index];
            if (token != GenericEndString)
                return System.Type.GetType(token);

            var types = CreateTypes(tokens, ref index);
            var lookupName = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}`{1}", tokens[tokens.Count - ++index], types.Count());
            return System.Type.GetType(lookupName).MakeGenericType(types);
        }

        private static System.Type[] CreateTypes(List<string> tokens, ref int index)
        {
            var types = new List<System.Type>();

            do
            {
                var type = CreateType(tokens, ref index);
                types.Insert(0, type);
            } while (tokens[tokens.Count - ++index] == GenericSeperatorString);

            return types.ToArray();
        }

        private static List<string> Tokenise(string transportableString)
        {
            var tokens = new List<string>();

            for (var index = 0; index != transportableString.Length; ++index)
            {
                var match = LabelRegex.Match(transportableString, index);
                if (match.Success && match.Index == index)
                {
                    tokens.Add(transportableString.Substring(index, match.Length));
                    index += match.Length - 1;
                    continue;
                }

                var currentChar = transportableString[index];
                if (currentChar == GenericBeginChar || currentChar == GenericSeperatorChar || currentChar == GenericEndChar)
                {
                    tokens.Add(new string(currentChar, 1));
                }
                else
                {
                    return new List<string>();
                }
            }

            return tokens;
        }

        private static TypeSerialiser GetCached(string transportableString)
        {
            lock (cacheLock)
            {
                TypeSerialiser cached;
                if (cachedStringSerialisers.TryGetValue(transportableString, out cached))
                {
                    return cached;
                }
            }

            return null;
        }

        private static TypeSerialiser GetCached(Type type)
        {
            lock (cacheLock)
            {
                TypeSerialiser cached;
                if (cachedTypeSerialisers.TryGetValue(type, out cached))
                {
                    return cached;
                }
            }

            return null;
        }

        #endregion Private Methods
    }
}