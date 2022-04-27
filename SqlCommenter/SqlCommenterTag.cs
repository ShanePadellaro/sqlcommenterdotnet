namespace SqlCommenter
{
    /// <summary>
    /// Use this class with EF's TagWith method to add data manually to the query
    /// </summary>
    public class SqlCommenterTag
    {
        private readonly string _route;
        private readonly string _controller;
        private readonly string _action;

        public SqlCommenterTag(string route, string controller, string action)
        {
            _route = route;
            _controller = controller;
            _action = action;
        }

        public string GetTagString()
        {
            return $"route='{_route}',controller='{_controller}',action='{_action}'";
        }
    }
}