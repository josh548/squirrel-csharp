using System.Collections.Generic;

namespace squirrel
{
    public class Environment
    {
        private readonly Environment _parent;
        private readonly Dictionary<string, AstNode> _definitions = new Dictionary<string, AstNode>();

        public Environment(Environment parent)
        {
            _parent = parent;
        }

        public Environment() : this(null)
        {
        }

        public void Put(string key, AstNode value) => _definitions.Add(key, value);

        public AstNode? Get(string key)
        {
            if (_parent == null)
            {
                return GetShallow(key);
            }
            return GetShallow(key) ?? _parent.Get(key);
        }

        private AstNode? GetShallow(string key)
        {
            if (_definitions.ContainsKey(key))
            {
                return _definitions[key];
            }
            return null;
        }
    }
}
