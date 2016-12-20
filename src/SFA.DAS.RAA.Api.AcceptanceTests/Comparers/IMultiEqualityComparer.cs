namespace SFA.DAS.RAA.Api.AcceptanceTests.Comparers
{
    public interface IMultiEqualityComparer<in T1, in T2>
    {
        bool Equals(T1 object1, T2 object2);

        int GetHashCode(T1 object1);

        int GetHashCode(T2 object2);
    }
}