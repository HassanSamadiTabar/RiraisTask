using Google.Protobuf;
using Grpc.Core;
using RiraisTask.Protos;

namespace WinFormClient
{
    public partial class UpdatePersonForm : Form
    {
        private readonly PeopleService.PeopleServiceClient _client;
        private readonly string _personId;
        private readonly Action? _onUpdated;
        private ByteString _rowVersion = ByteString.Empty;

        public UpdatePersonForm(
            PeopleService.PeopleServiceClient client,
            string personId,
            Action? onUpdated = null)
        {
            _client = client;
            _personId = personId;
            _onUpdated = onUpdated;
            InitializeComponent();
            LoadPerson();
        }

        private void LoadPerson()
        {
            try
            {
                var person = _client.GetPerson(new GetPersonRequest { Id = _personId });

                FirstName.Text = person.FirstName;
                Lastname.Text = person.LastName;
                NationalCode.Text = person.NationalCode;
                BirthDate.Value = DateTime.Parse(person.BirthDate);
                _rowVersion = person.RowVersion;
            }
            catch (RpcException ex)
            {
                MessageBox.Show(
                    ex.Status.Detail,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                DialogResult = DialogResult.Cancel;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Unexpected Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                _client.UpdatePerson(new UpdatePersonRequest
                {
                    Id = _personId,
                    FirstName = FirstName.Text,
                    LastName = Lastname.Text,
                    NationalCode = NationalCode.Text,
                    BirthDate = BirthDate.Value.ToString("yyyy-MM-dd"),
                    RowVersion = _rowVersion
                });

                MessageBox.Show(
                    "Person updated successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                _onUpdated?.Invoke();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (RpcException ex)
            {
                MessageBox.Show(
                    ex.Status.Detail,
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Unexpected Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void NationalCode_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
