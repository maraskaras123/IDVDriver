namespace IDVDriver.Tests
{
    public abstract class IDVDriverTestBase : TestBase
    {
        // insert
        public abstract void Can_insert_item_to_database();

        // get 
        public abstract void Can_get_item_by_id();
        
        // get all
        public abstract void Can_get_items();

        // delete
        public abstract void Can_delete_item();

        // update
        public abstract void Can_update_item();
    }
}