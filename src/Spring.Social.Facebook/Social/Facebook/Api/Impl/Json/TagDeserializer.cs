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

using System;
using System.Globalization;
using System.Collections.Generic;

using Spring.Json;

namespace Spring.Social.Facebook.Api.Impl.Json
{
	/// <summary>
	/// JSON deserializer for Tag. 
	/// </summary>
	/// <author>Craig Walls</author>
	/// <author>SplendidCRM (.NET)</author>
	class TagDeserializer : IJsonDeserializer
	{
		public object Deserialize(JsonValue json, JsonMapper mapper)
		{
			Tag video = null;
			if ( json != null && !json.IsNull )
			{
				video = new Tag();
				video.ID          = json.ContainsName("id"          ) ? json.GetValue<string>("id"  ) : String.Empty;
				video.Name        = json.ContainsName("name"        ) ? json.GetValue<string>("name") : String.Empty;
				video.X           = json.ContainsName("x"           ) ? json.GetValue<int   >("x"   ) : 0;
				video.Y           = json.ContainsName("y"           ) ? json.GetValue<int   >("y"   ) : 0;
				video.CreatedTime = json.ContainsName("created_time") ? JsonUtils.ToDateTime(json.GetValue<string>("created_time"), "yyyy-MM-ddTHH:mm:ss") : DateTime.MinValue;
			}
			return video;
		}
	}
}
