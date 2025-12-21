namespace F1Simulator.Utils.DatabaseConnectionFactory
{
    public interface IDatabaseConnection<TConnection>
    {
        TConnection Connect();
    }
}
