using System;
using System.Collections.Generic;
using Core.Extensions;
using UnityEngine;

namespace Core.Global.Network
{
	[Serializable]
	public class Request
	{
		[SerializeField]
		public string Id;

		[SerializeField]
		public string RequestType;

		[SerializeField] public List<RequestParameter> Parameters;

		[SerializeField]
		public bool WithResponse;

		[SerializeField]
		public bool IsResponse;

		public Request(string requestType, List<RequestParameter> parameters)
		{
			Id = Guid.NewGuid().ToString();

			RequestType = requestType;
			Parameters = parameters;
		}

		public static byte[] SerializeToBytes(Request request)
		{
			Debug.Log("Сериализация запроса :" + request.RequestType);
		
			byte[] bytes = BinaryExtension.SerializeToBytes(request);
			
			return bytes;
		}

		public static Request DeSerializeToBytes(byte[]  bytes)
		{
			Request request = BinaryExtension.DeserializeToObject<Request>(bytes);

			Debug.Log("Десериализация запроса :" + request.RequestType);

			return request;
		}
	}

	[Serializable]
	public class RequestParameter
	{
		[SerializeField]
		public object Value;

		[SerializeField]
		public Type Type;

		public RequestParameter(object value)
		{
			Value = value;
			Type = value.GetType();
		}
	}
}