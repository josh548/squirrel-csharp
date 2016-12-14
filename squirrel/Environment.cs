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

        public void Add(string name, AstNode value)
        {
            _definitions.Add(name, value);
        }

        public AstNode? Get(string name)
        {
            return GetShallow(name) ?? _parent.Get(name);
        }

        private AstNode? GetShallow(string name)
        {
            if (_definitions.ContainsKey(name))
            {
                return _definitions[name];
            }
            return null;
        }
    }
}
