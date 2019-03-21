﻿#region License

/*
 * Copyright 2002-2012 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      https://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System.Collections.Generic;

using Spring.Json;

namespace Spring.Social.Facebook.Api.Impl.Json
{
	/// <summary>
	/// JSON deserializer a generic list.
	/// </summary>
	/// <author>Bruno Baia</author>
	/// <author>SplendidCRM (.NET)</author>
	class ListDeserializer<T> : IJsonDeserializer
	{
		public object Deserialize(JsonValue json, JsonMapper mapper)
		{
			IList<T> entries = null;
			if ( json != null && !json.IsNull )
			{
				JsonValue dataNode = json.GetValue("data");
				if ( dataNode != null )
				{
					entries = new List<T>();
					foreach ( JsonValue itemValue in dataNode.GetValues() )
					{
						entries.Add(mapper.Deserialize<T>(itemValue));
					}
				}
			}
			return entries;
		}
	}
}
