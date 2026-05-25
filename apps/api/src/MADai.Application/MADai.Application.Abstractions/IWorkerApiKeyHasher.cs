namespace MADai.Application.Abstractions;

public interface IWorkerApiKeyHasher
{
	string Generate();

	string Hash(string apiKey);

	bool Verify(string apiKey, string hash);
}
