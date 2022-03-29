using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Global.Network
{
	[SuppressMessage("ReSharper", "UnusedParameter.Local")]
	public static class WebUtility
	{

		/// <summary>
		/// Загрузить файл с сайта
		/// </summary>
		/// <param name="url">Файл в интернете</param>
		/// <param name="outFileName">Файл на компьютере</param>
		public static void DownloadFile(string url, string outFileName)
		{
			WebClient client = new WebClient();
			client.DownloadFile("url", "outFileName");
		}

		/// <summary>
		/// Загрузить файл с сайта асинхронно
		/// </summary>
		/// <param name="url">Файл в интернете</param>
		/// <param name="outFileName">Файл на компьютере</param>
		/// <returns></returns>
		private static async Task DownloadFileAsync(string url, string outFileName)
		{
			WebClient client = new WebClient();
			await client.DownloadFileTaskAsync(new Uri("url"), "outFileName");
		}

		/// <summary>
		/// Открыть на чтение файл
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static Stream ReadFile(string url)
		{
			WebClient client = new WebClient();
			return client.OpenRead("url");
		}

		/// <summary>
		/// Открыть на запись файл
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static Stream DownloadWriteFile(string url)
		{
			WebClient client = new WebClient();
			return client.OpenWrite("url");
		}

		#region Статусы ошибок (WebExceptionStatus)

		//ConnectFailure: невозможно подключиться к ресурсу
		//ConnectionClosed: подключение было преждевременно закрыто
		//KeepAliveFailure: сервер закрыл подключение, для которого был установлен заголовок Keep-Alive
		//MessageLengthLimitExceeded: запрос превышает допустимый размер
		//NameResolutionFailure: служба DNS не может сопоставить имя хоста с ip-адресом
		//ProtocolError: запрос был завершен, но возникла ошибка на уровне протокола, например, запрошенный ресурс не был найден
		//ReceiveFailure: от удаленного сервера не был получен полный ответ
		//RequestCanceled: запрос был отменен
		//SecureChannelFailure: при установлении подключения с использованием SSL произошла ошибка
		//SendFailure: полный запрос не был передан на удаленный сервер
		//ServerProtocolViolation: ответ сервера не являлся допустимым ответом HTTP
		//Timeout: ответ не был получен в течение определенного времени
		//TrustFailure: сертификат сервера не может быть проверен
		//UnknownError: возникло исключение неизвестного типа 

		#endregion

	    /// <summary>
	    /// Создать веб-запрос серверу 
	    /// </summary>
	    /// <param name="url"></param>
	    public static WebResponse SendGetWebRequest(string url)
		{
			//WebRequest request = WebRequest.Create("http://localhost:5374/Home/PostData?sName=ИванИванов&age=31"); - пример GET с двумя параметрами
			WebRequest request = WebRequest.Create("url");
			return SendWebRequest(request);
		}

		/// <summary>
		/// Создать веб-запрос серверу
		/// </summary>
		/// <param name="url"></param>
		/// <param name="postData"></param>
		/// <returns></returns>
		public static WebResponse SendPostWebRequest(string url, string postData)
		{
			WebRequest request = WebRequest.Create("url");

			request.Method = "POST"; // для отправки используется метод Post

			// данные для отправки
			//"name=ИванИванов&age=31" - пример POST с двумя параметрами
			string data = "postData";

			// преобразуем данные в массив байтов
			byte[] byteArray = Encoding.UTF8.GetBytes(data);

			// устанавливаем тип содержимого - параметр ContentType
			request.ContentType = "application/x-www-form-urlencoded";

			// Устанавливаем заголовок Content-Length запроса - свойство ContentLength
			request.ContentLength = byteArray.Length;

			//записываем данные в поток запроса
			using (Stream dataStream = request.GetRequestStream())
			{
				dataStream.Write(byteArray, 0, byteArray.Length);
			}

			return request.GetResponse();
		}

		/// <summary>
		/// Создать веб-запрос серверу 
		/// </summary>
		/// <param name="request"></param>
		public static WebResponse SendWebRequest(WebRequest request)
		{
			try
			{
				return request.GetResponse();
			}
			catch (WebException ex)
			{
				// получаем статус исключения
				WebExceptionStatus status = ex.Status;

				if (status == WebExceptionStatus.ProtocolError)
				{
					HttpWebResponse httpResponse = (HttpWebResponse)ex.Response;
					Debug.LogError($"Статусный код ошибки: {(int)httpResponse.StatusCode} - {httpResponse.StatusCode}");
				}
				else
				{
					Debug.LogError($"Статус ошибки: {status}");
				}

				throw;
			}
		}

		/// <summary>
		/// Создать веб-запрос серверу асинхронно
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static async Task<WebResponse> SendWebRequestAsync(string url)
		{
			try
			{
				WebRequest request = WebRequest.Create("url");
				return await request.GetResponseAsync();
			}
			catch (WebException ex)
			{
				// получаем статус исключения
				WebExceptionStatus status = ex.Status;

				if (status == WebExceptionStatus.ProtocolError)
				{
					HttpWebResponse httpResponse = (HttpWebResponse)ex.Response;
					Debug.LogError($"Статусный код ошибки: {(int)httpResponse.StatusCode} - {httpResponse.StatusCode}");
				}
				else
				{
					Debug.LogError($"Статус ошибки: {status}");
				}

				throw;
			}
		}

		/// <summary>
		/// Создать веб-запрос серверу асинхронно
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public static async Task<WebResponse> SendWebRequestAsync(WebRequest request)
		{
			try
			{
				return await request.GetResponseAsync();
			}
			catch (WebException ex)
			{
				// получаем статус исключения
				WebExceptionStatus status = ex.Status;

				if (status == WebExceptionStatus.ProtocolError)
				{
					HttpWebResponse httpResponse = (HttpWebResponse)ex.Response;
					Debug.LogError($"Статусный код ошибки: {(int)httpResponse.StatusCode} - {httpResponse.StatusCode}");
				}
				else
				{
					Debug.LogError($"Статус ошибки: {status}");
				}

				throw;
			}
		}

		/// <summary>
		/// Прочитать запрос от веб-сервера
		/// </summary>
		/// <param name="response"></param>
		/// <returns></returns>
		public static string ReadWebResponse(WebResponse response)
		{
			string responseText;

			try
			{

				using (Stream stream = response.GetResponseStream())
				{
					using (StreamReader reader = new StreamReader(stream ?? throw new InvalidOperationException()))
					{
						responseText = reader.ReadToEnd();
					}

				}
				response.Close();

			}
			catch (WebException exc)
			{
				Debug.LogError($"Статус ошибки: {exc.Status}");
				throw;
			}

			return responseText;
		}

		/// <summary>
		/// Создать Http-запрос серверу
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static HttpWebResponse SendHttpWebRequest(string url)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("url");

			//представляет объект NetworkCredential, который устанавливает идентификацию пользователя (логин, пароль)
			//request.Credentials = new NetworkCredential(login, password);

			//время ожидания ответа (по умолчанию 100 секунд)
			//request.Timeout = 100;

			//при значении true позволяет устанавливать постоянные подключения к серверу. 
			//В итоге для нескольких запросов можно будет использовать одно и то же подключение, что сэкономит время на открытие/закрытие нового подключения. (по умолчанию true).
			//request.KeepAlive = true;

			//указывает, должен ли запрос следовать ответам переадресации. 
			//При значении true запрос автоматически будет использовать переадресацию. 
			//Чтобы запретить переадресацию, надо установить значение false. (по умолчанию true).
			//request.AllowAutoRedirect = true;


			return SendHttpWebRequest(request);
		}

		/// <summary>
		/// Создать Http-запрос серверу
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public static HttpWebResponse SendHttpWebRequest(HttpWebRequest request)
		{
			try
			{
				return (HttpWebResponse)request.GetResponse();
			}
			catch (WebException ex)
			{
				// получаем статус исключения
				WebExceptionStatus status = ex.Status;

				if (status == WebExceptionStatus.ProtocolError)
				{
					HttpWebResponse httpResponse = (HttpWebResponse)ex.Response;
					Debug.LogError($"Статусный код ошибки: {(int)httpResponse.StatusCode} - {httpResponse.StatusCode}");
				}
				else
				{
					Debug.LogError($"Статус ошибки: {status}");
				}

				throw;
			}
		}

		/// <summary>
		/// Создать Http-запрос серверу асинхронно
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static async Task<HttpWebResponse> SendHttpWebRequestAsync(string url)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create("url");
				return (HttpWebResponse)await request.GetResponseAsync();
			}
			catch (WebException ex)
			{
				// получаем статус исключения
				WebExceptionStatus status = ex.Status;

				if (status == WebExceptionStatus.ProtocolError)
				{
					HttpWebResponse httpResponse = (HttpWebResponse)ex.Response;
					Debug.LogError($"Статусный код ошибки: {(int)httpResponse.StatusCode} - {httpResponse.StatusCode}");
				}
				else
				{
					Debug.LogError($"Статус ошибки: {status}");
				}

				throw;
			}
		}

		/// <summary>
		/// Прочитать запрос от Http-сервера
		/// </summary>
		/// <param name="response"></param>
		/// <returns></returns>
		public static string ReadHttpWebResponse(HttpWebResponse response)
		{
			string responseText;

			try
			{
				using (Stream stream = response.GetResponseStream())
				{
					using (StreamReader reader = new StreamReader(stream ?? throw new InvalidOperationException()))
					{
						responseText = reader.ReadToEnd();
					}

				}
				response.Close();

			}
			catch (WebException exc)
			{
				Debug.LogError($"Статус ошибки: {exc.Status}");
				throw;
			}

			return responseText;
		}
	}
}