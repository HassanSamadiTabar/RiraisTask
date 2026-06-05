using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using RiraisTask.Protos;

namespace WinFormClient
{
    public partial class AddAndGetPersonForm : Form
    {
        private readonly GrpcChannel channel;
        private readonly PeopleService.PeopleServiceClient client;

        public AddAndGetPersonForm()
        {
            InitializeComponent();
            channel = GrpcChannel.ForAddress("https://localhost:7246");
            client = new PeopleService.PeopleServiceClient(channel);
            SetupGrid();
        }

        private void SetupGrid()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "Id",
                Name = "Id",
                Visible = false
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FirstName",
                HeaderText = "نام",
                Name = "FirstName",
                ReadOnly = true
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "LastName",
                HeaderText = "نام خانوادگی",
                Name = "LastName",
                ReadOnly = true
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NationalCode",
                HeaderText = "کد ملی",
                Name = "NationalCode",
                ReadOnly = true
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "BirthDate",
                HeaderText = "تاریخ تولد",
                Name = "BirthDate",
                ReadOnly = true
            });
            dataGridView1.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "ویرایش",
                Name = "Edit",
                Text = "ویرایش",
                UseColumnTextForButtonValue = true
            });
            dataGridView1.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "حذف",
                Name = "Delete",
                Text = "حذف",
                UseColumnTextForButtonValue = true
            });
        }

        private void LoadPeople()
        {
            var response = client.GetAllPeople(new GetAllPeopleRequest
            {
                Page = 1,
                PageSize = 200
            });
            dataGridView1.DataSource = response.People.Select(p => new PersonGridRow
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                NationalCode = p.NationalCode,
                BirthDate = p.BirthDate
            }).ToList();
        }

        private void OpenUpdateForm(string personId)
        {
            using var updateForm = new UpdatePersonForm(client, personId, LoadPeople);
            updateForm.ShowDialog(this);
        }

        private void DeletePerson(string personId)
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to delete this person?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                client.DeletePerson(new DeletePersonRequest { Id = personId });

                MessageBox.Show(
                    "Person deleted successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadPeople();
            }
            catch (RpcException ex)
            {
                MessageBox.Show(
                    ex.Status.Detail,
                    "Error",
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var response = client.CreatePerson(new CreatePersonRequest
                {
                    FirstName = FirstName.Text,
                    LastName = Lastname.Text,
                    NationalCode = NationalCode.Text,
                    BirthDate = BirthDate.Value.ToString("yyyy-MM-dd")
                });

                MessageBox.Show(
                    $"Person created successfully.\nId: {response.Id}",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadPeople();
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

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadPeople();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            var columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (columnName is not ("Edit" or "Delete"))
            {
                return;
            }

            var personId = dataGridView1.Rows[e.RowIndex].Cells["Id"].Value?.ToString();
            if (string.IsNullOrEmpty(personId))
            {
                return;
            }

            if (columnName == "Edit")
            {
                OpenUpdateForm(personId);
            }
            else
            {
                DeletePerson(personId);
            }
        }

        private sealed class PersonGridRow
        {
            public string Id { get; set; } = string.Empty;
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string NationalCode { get; set; } = string.Empty;
            public string BirthDate { get; set; } = string.Empty;
        }

        private void FirstName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
