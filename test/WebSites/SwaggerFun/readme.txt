foreach path in document.paths
	foreach operation in path.operations
		foreach parameter in operation.parameters // includes path, query etc.
			parameter.generateSchema()
			parameter.callParameterFilters()
		
		operation.requestBody.generateSchema()
		
		foreach response in operation.responses
			foreach contentType in response.contentTypes
				response.responseBody.generateSchema(contentType)
				
		operation.callOperationFilters()

document.callDocumentFilters()

each call to generateSchema() invokes schemaFilters



if one of the parameters or models is IEnumerable<Foo>

	public class Foo
	{
		public string A { get; set; }
		
		public string B { get; set; }
	}

then schema generation is recursive:

	1) Called on each property (A, B...)
	2) Called on Foo
	3) Called on IEnumerable<Foo>
	
