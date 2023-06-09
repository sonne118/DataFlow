
// BufferBlock_BoundCapacity
var boundedCapacity = 1;
var bb = new BufferBlock<int>(new DataflowBlockOptions() { BoundedCapacity = 4 });

var a1 = new ActionBlock<int>(
	a =>
	{
		Console.WriteLine("Action A1 executing with value {0}", a);
		Thread.Sleep(100);
	}
	, new ExecutionDataflowBlockOptions() { BoundedCapacity = boundedCapacity }
);

var a2 = new ActionBlock<int>(
	a =>
	{
		Console.WriteLine("Action A2 executing with value {0}", a);
		Thread.Sleep(50);
	}
	, new ExecutionDataflowBlockOptions() { BoundedCapacity = boundedCapacity }
);
var a3 = new ActionBlock<int>(
	a =>
	{
		Console.WriteLine("Action A3 executing with value {0}", a);
		Thread.Sleep(200);
	}
	, new ExecutionDataflowBlockOptions() { BoundedCapacity = boundedCapacity }
);

var brc = new BroadcastBlock<int>(n => n);

bb.LinkTo(brc);
brc.LinkTo(a1);
brc.LinkTo(a2);
brc.LinkTo(a3);

for (int i = 0; i < 10; i++)
{
	Thread.Sleep(10);
	bb
		.SendAsync(i)
		.ContinueWith(a => Console.WriteLine($"Message {i} sent #{a.Result}"));
}
