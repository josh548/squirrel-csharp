using System.Collections.Generic;
using squirrel.Nodes;

namespace squirrel
{
    public class Environment
    {
        private readonly Environment _parent;
        private readonly Dictionary<string, INode> _definitions = new Dictionary<string, INode>();

        public Environment(Environment parent)
        {
            _parent = parent;
        }

        public Environment() : this(null)
        {
        }

        public void Put(string key, INode value) => _definitions.Add(key, value);

        public INode Get(string key)
        {
            if (_parent == null)
            {
                return GetShallow(key);
            }
            return GetShallow(key) ?? _parent.Get(key);
        }

        private INode GetShallow(string key)
        {
            return _definitions.ContainsKey(key) ? _definitions[key] : null;
        }
    }
}
