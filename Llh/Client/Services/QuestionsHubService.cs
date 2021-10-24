using Llh.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Llh.Client.Services
{
    public sealed class QuestionsHubService : IAsyncDisposable
    {
        private readonly HubConnection hubConnection;

        public QuestionsHubService(NavigationManager navigationMannager)
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(navigationMannager.ToAbsoluteUri("/questions-hub"))
                .Build();

            hubConnection.On<StudentQuestionDto>("NewStudentQuestion", newQuestion =>
            {
                NewStudentQuestion.Invoke(newQuestion);
            });

            hubConnection.On<StudentQuestionDto>("StudentQuestionAnswered", question =>
            {
                StudentQuestionAnswered.Invoke(question.Id);
            });

            hubConnection.On<LecturerQuestionDto>("NewLecturerQuestion", question =>
            {
                NewLecturerQuestion.Invoke(question);
            });

            hubConnection.On<LecturerQuestionDto>("LecturerQuestionMarkClosed", question =>
            {
                LecturerQuestionMarkClosed.Invoke(question.Id);
            });

            hubConnection.On<LecturerQuestionAnswerDto>("LecturerQuestionAddAnswer", question =>
            {
                LecturerQuestionAddAnswer.Invoke(question);
            });

            hubConnection.StartAsync();
        }

        public delegate void NewStudentQuestionHandler(StudentQuestionDto studentQuestion);
        public event NewStudentQuestionHandler NewStudentQuestion;

        public delegate void StudentQuestionAnsweredHandler(int questionId);
        public event StudentQuestionAnsweredHandler StudentQuestionAnswered;


        public delegate void NewLecturerQuestionHandler(LecturerQuestionDto lecturerQuestion);
        public event NewLecturerQuestionHandler NewLecturerQuestion;

        public delegate void LecturerQuestionMarkClosedHandler(int questionId);
        public event LecturerQuestionMarkClosedHandler LecturerQuestionMarkClosed;

        public delegate void LecturerQuestionAddAnswerHandler(LecturerQuestionAnswerDto questionAnswer);
        public event LecturerQuestionAddAnswerHandler LecturerQuestionAddAnswer;

        public async ValueTask DisposeAsync()
        {
             await hubConnection.DisposeAsync();
        }
    }
}
