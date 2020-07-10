namespace XsarilAI.Configuration.Reader {
	public interface IConfigurationReaderProvider {

		IConfigurationReader GetReader(string filename);

	}
}
