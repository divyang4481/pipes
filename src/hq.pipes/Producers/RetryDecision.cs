namespace hq.pipes.Producers
{
    public enum RetryDecision
    {
        RetryImmediately,
        Requeue,
        Backlog,
        Undeliverable,
        Destroy
    }
}