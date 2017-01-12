using System.Collections.Generic;
using Squirrel.Nodes;

namespace Squirrel
{
    public class Environment
    {
        public readonly Environment Parent;
        private readonly Dictionary<string, INode> _definitions = new Dictionary<string, INode>();

        public Environment(Environment parent)
        {
            Parent = parent;
        }

        public Environment() : this(null)
        {
        }

        public void Put(string key, INode value) => _definitions[key] = value;

        public void PutParent(string key, INode value) => Parent.Put(key, value);

        public void PutGrandparent(string key, INode value) => Parent.PutParent(key, value);

        public INode Get(string key)
        {
            if (Parent == null)
            {
                return GetShallow(key);
            }
            return GetShallow(key) ?? Parent.Get(key);
        }

        private INode GetShallow(string key) => _definitions.ContainsKey(key) ? _definitions[key] : null;

        public void Extend(Environment env)
        {
            foreach (var definition in env._definitions)
            {
                Put(definition.Key, definition.Value);
            }
        }
    }
}
