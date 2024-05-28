CREATE TABLE [dbo].[SmartHomeData](
	[Idx] [bigint] IDENTITY(1,1) NOT NULL,
	[DEV_ID] [varchar](10) NOT NULL,
	[CURR_DT] [datetime] NOT NULL,
	[TEMP] [float] NOT NULL,
	[HUMID] [float] NOT NULL,
 CONSTRAINT [PK_SmartHomeData] PRIMARY KEY CLUSTERED 
(
	[Idx] ASC
)
) ON [PRIMARY]
GO


