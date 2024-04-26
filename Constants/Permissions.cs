namespace FirstWebApplication.Constants
{
    public static class Permissions<TModule>
    {
        public static class Module
        {
            public static string View => $"Permissions.{typeof(TModule).Name}.View";
            public static string Create => $"Permissions.{typeof(TModule).Name}.Create";
            public static string Edit => $"Permissions.{typeof(TModule).Name}.Edit";
            public static string Delete => $"Permissions.{typeof(TModule).Name}.Delete";
        }

        public static List<string> GeneratePermissionsForModule(string module)
        {
            var permissions = new List<string>()
        {
            $"Permissions.{module}.View",
            $"Permissions.{module}.Create",
            $"Permissions.{module}.Edit",
            $"Permissions.{module}.Delete",
        };

            return permissions;
        }
    }

    //public static class Permissions
    //{
    //    public static List<string> GeneratePermissionsForModule(string module)
    //    {
    //        return new List<string>()
    //    {
    //        $"Permissions.{module}.Create",
    //        $"Permissions.{module}.View",
    //        $"Permissions.{module}.Edit",
    //        $"Permissions.{module}.Delete",
    //    };
    //    }
    //    public static class Products
    //    {
    //        public const string View = "Permissions.Products.View";
    //        public const string Create = "Permissions.Products.Create";
    //        public const string Edit = "Permissions.Products.Edit";
    //        public const string Delete = "Permissions.Products.Delete";
    //    }


    //}
}
