using System.IO;
using hq.pipes.Consumers;
using hq.pipes.Serializers;
using hq.pipes.tests.Fakes;
using hq.pipes.tests.Fixtures;
using Xunit;

namespace hq.pipes.tests
{
    public class FileConsumerTests : IClassFixture<FileFolderFixture>
    {
        private readonly FileFolderFixture _fixture;

        public FileConsumerTests(FileFolderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Events_persist_as_json_on_disk()
        {
            PersistsAsSerialized(new JsonSerializer(), ".json");
        }

        private async void PersistsAsSerialized(ISerializer serializer, string extension)
        {
            var consumer = new FileConsumer<StringEvent>(serializer, _fixture.Folder);
            var @event = new StringEvent("Test!");
            await consumer.HandleAsync(@event);

            var file = OneFileSaved(extension);
            FileContainsTheEvent(file, serializer, @event);
        }

        private static void FileContainsTheEvent<T>(string file, ISerializer serializer, T @event)
        {
            var expected = @event.ToString();

            T deserialized;
            using (var fs = File.OpenRead(file))
                deserialized = serializer.DeserializeFromStream<T>(fs);
            Assert.NotNull(deserialized);

            var actual = deserialized.ToString();
            Assert.Equal(expected, actual);
        }

        private string OneFileSaved(string extension)
        {
            var files = Directory.GetFiles(_fixture.Folder, "*" + extension);
            Assert.Equal(1, files.Length);
            var file = files[0];
            return file;
        }
    }
}