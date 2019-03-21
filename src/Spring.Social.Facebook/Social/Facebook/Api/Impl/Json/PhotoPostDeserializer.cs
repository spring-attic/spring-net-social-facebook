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
	/// JSON deserializer for PhotoPost. 
	/// </summary>
	/// <author>Craig Walls</author>
	/// <author>SplendidCRM (.NET)</author>
	class PhotoPostDeserializer : IJsonDeserializer
	{
		public object Deserialize(JsonValue json, JsonMapper mapper)
		{
			PhotoPost post = null;
			if ( json != null && !json.IsNull )
			{
				post = new PhotoPost();
				post.ID          = json.ContainsName("id"          ) ? json.GetValue<string>("id"       ) : String.Empty;
				post.PhotoId     = json.ContainsName("object_id"   ) ? json.GetValue<string>("object_id") : String.Empty;
				post.CreatedTime = json.ContainsName("created_time") ? JsonUtils.ToDateTime(json.GetValue<string>("created_time"), "yyyy-MM-ddTHH:mm:ss") : DateTime.MinValue;
				post.UpdatedTime = json.ContainsName("updated_time") ? JsonUtils.ToDateTime(json.GetValue<string>("updated_time"), "yyyy-MM-ddTHH:mm:ss") : DateTime.MinValue;
				
				post.From        = mapper.Deserialize<Reference>(json.GetValue("from"));
				post.Tags        = mapper.Deserialize<List<Tag>>(json.GetValue("tags"));
			}
			return post;
		}
	}
}
