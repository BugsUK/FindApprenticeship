﻿SET IDENTITY_INSERT [dbo].[ApprenticeshipFramework] ON
GO

MERGE INTO [dbo].[ApprenticeshipFramework] AS Target 
USING (VALUES 
(1,	1,	'102', '102', 'Business and Administration',	2,	NULL,	NULL),
(2,	20,	'258', '258', 'Advice and Guidance',	2,	NULL,	NULL),
(3,	1,	'260', '260', 'Team Leading and Management',	2,	NULL,	NULL),
(4,	22,	'301', '301', 'Learning and Development (Direct Training and Support)',	2,	NULL,	NULL),
(5,	2,	'101', '101', 'Agriculture',	2,	NULL,	NULL),
(6,	2,	'219', '219', 'Amenity Horticulture',	2,	NULL,	NULL),
(7,	2,	'233', '233', 'Trees and Timber',	2,	NULL,	NULL),
(8,	2,	'243', '243', 'Environmental Conservation',	2,	NULL,	NULL),
(9,	2,	'244', '244', 'Floristry',	2,	NULL,	NULL),
(10,	2,	'247', '247', 'Equine',	2,	NULL,	NULL),
(11,	2,	'254', '254', 'Land Based Engineering',	2,	NULL,	NULL),
(12,	2,	'262', '262', 'Animal Care',	2,	NULL,	NULL),
(13,	2,	'284', '284', 'Fencing',	2,	NULL,	NULL),
(14,	2,	'299', '299', 'Farriery',	2,	NULL,	NULL),
(15,	2,	'303', '303', 'Production Horticulture',	2,	NULL,	NULL),
(16,	17,	'309', '309', 'Saddlery',	2,	NULL,	NULL),
(17,	2,	'314', '314', 'Dry Stone Walling',	2,	NULL,	NULL),
(18,	2,	'316', '316', 'Game and Wildlife Management',	2,	NULL,	NULL),
(19,	2,	'317', '317', 'Veterinary Nursing',	2,	NULL,	NULL),
(20,	2,	'342', '342', 'Animal Technology',	1,	'2013-07-01 00:00:00.000',	NULL),
(21,	17,	'276', '276', 'Textiles',	2,	NULL,	NULL),
(22,	3,	'321', '321', 'Design',	2,	NULL,	NULL),
(23,	3,	'361', '361', 'Creative',	2,	NULL,	NULL),
(24,	14,	'202', '202', 'Aviations Operations on the Ground',	2,	NULL,	NULL),
(25,	17,	'206', '206', 'Transport Engineering and Maintenance',	2,	NULL,	NULL),
(26,	17,	'278', '278', 'Rail Transport Engineering',	2,	NULL,	NULL),
(27,	17,	'300', '300', 'Rail Services',	2,	NULL,	NULL),
(28,	17,	'310', '310', 'Passenger Carrying Vehicles Driving  - Bus and Coach',	2,	NULL,	NULL),
(29,	17,	'327', '327', 'Retail Motor Industry: Vehicle Fitting',	2,	NULL,	NULL),
(30,	17,	'328', '328', 'Retail Motor Industry: Vehicle Maintenance and Repair',	2,	NULL,	NULL),
(31,	17,	'329', '329', 'Retail Motor Industry: Roadside Assistance and Recovery',	2,	NULL,	NULL),
(32,	17,	'330', '330', 'Retail Motor Industry: Vehicle Body and Paint Operations',	2,	NULL,	NULL),
(33,	17,	'331', '331', 'Retail Motor Industry: Vehicle Parts Operation',	2,	NULL,	NULL),
(34,	13,	'332', '332', 'Retail Motor Industry: Vehicle Sales',	2,	NULL,	NULL),
(35,	14,	'349', '349', 'Cabin Crew',	2,	NULL,	NULL),
(36,	3,	'353', '353', 'Games Testing',	2,	NULL,	NULL),
(37,	7,	'364', '364', 'Set Crafts',	2,	NULL,	NULL),
(38,	17,	'103', '103', 'Process Technology',	2,	NULL,	NULL),
(39,	17,	'111', '111', 'Polymer Processing Operations',	2,	NULL,	NULL),
(40,	17,	'280', '280', 'Signmaking',	2,	NULL,	NULL),
(41,	17,	'357', '357', 'Nuclear Decommissioning',	2,	NULL,	NULL),
(42,	17,	'107', '107', 'Engineering Construction',	2,	NULL,	NULL),
(43,	7,	'116', '116', 'Construction',	2,	NULL,	NULL),
(44,	7,	'343', '343', 'Highways Maintenance',	2,	'2009-08-06 00:00:00.000',	NULL),
(45,	7,	'344', '344', 'Fitted Interiors',	2,	'2009-08-06 00:00:00.000',	NULL),
(46,	7,	'352', '352', 'Industrial Building Systems',	2,	'2009-08-06 00:00:00.000',	NULL),
(47,	7,	'355', '355', 'Construction Diploma',	2,	'2009-08-06 00:00:00.000',	NULL),
(48,	13,	'112', '112', 'Retail',	2,	NULL,	NULL),
(49,	1,	'263', '263', 'Customer Service',	2,	NULL,	NULL),
(50,	1,	'293', '293', 'Contact Centres',	2,	NULL,	NULL),
(51,	1,	'304', '304', 'Sales & Telesales',	2,	NULL,	NULL),
(52,	17,	'211', '211', 'Power Industry',	2,	NULL,	NULL),
(53,	17,	'265', '265', 'Gas Industry',	2,	NULL,	NULL),
(54,	17,	'277', '277', 'Water Industry',	2,	NULL,	NULL),
(55,	17,	'347', '347', 'Gas Network Operations',	2,	NULL,	NULL),
(56,	7,	'105', '105', 'Electrotechnical',	2,	NULL,	NULL),
(57,	17,	'106', '106', 'Engineering',	2,	NULL,	NULL),
(58,	7,	'117', '117', 'Plumbing',	2,	NULL,	NULL),
(59,	7,	'212', '212', 'Heating, Ventilation, Air Conditioning and Refrigeration',	2,	NULL,	NULL),
(60,	17,	'282', '282', 'Building Services Engineering Technicians',	2,	NULL,	NULL),
(61,	17,	'283', '283', 'Industrial Applications',	2,	NULL,	NULL),
(62,	20,	'286', '286', 'Optical',	2,	NULL,	NULL),
(63,	7,	'294', '294', 'Electrical and Electronic Servicing',	2,	NULL,	NULL),
(64,	17,	'336', '336', 'Engineering Technology',	2,	NULL,	NULL),
(65,	17,	'338', '338', 'Munition Clearance and Search Occupations',	2,	'2009-08-06 00:00:00.000',	NULL),
(66,	1,	'201', '201', 'Accounting',	2,	NULL,	NULL),
(67,	1,	'296', '296', 'Payroll',	2,	NULL,	NULL),
(68,	1,	'346', '346', 'Providing Financial Services',	2,	NULL,	NULL),
(69,	1,	'351', '351', 'Advising on Financial Products',	2,	NULL,	NULL),
(70,	17,	'354', '354', 'Food Manufacture',	2,	NULL,	NULL),
(71,	13,	'217', '217', 'Hairdressing',	2,	NULL,	NULL),
(72,	13,	'279', '279', 'Beauty Therapy',	2,	NULL,	NULL),
(73,	13,	'305', '305', 'Barbering',	2,	NULL,	NULL),
(74,	13,	'318', '318', 'Nail Services',	2,	'2009-08-06 00:00:00.000',	NULL),
(75,	13,	'350', '350', 'Spa Therapy',	2,	NULL,	NULL),
(76,	14,	'115', '115', 'Travel Services',	2,	NULL,	NULL),
(77,	13,	'220', '220', 'Hospitality and Catering',	2,	NULL,	NULL),
(78,	14,	'289', '289', 'Events',	2,	NULL,	NULL),
(79,	15,	'322', '322', 'IT Users',	2,	NULL,	NULL),
(80,	15,	'341', '341', 'ICT Professional',	2,	NULL,	NULL),
(81,	15,	'360', '360', 'IT and Telecomms Professionals',	2,	NULL,	NULL),
(82,	13,	'228', '228', 'Carry and Deliver Goods',	2,	NULL,	NULL),
(83,	17,	'295', '295', 'Driving Goods Vehicles',	2,	NULL,	NULL),
(84,	17,	'324', '324', 'Traffic Office',	2,	NULL,	NULL),
(85,	13,	'334', '334', 'Mail Services',	2,	NULL,	NULL),
(86,	13,	'362', '362', 'Purchasing and Supply Management',	2,	NULL,	NULL),
(87,	13,	'363', '363', 'Warehousing and Storage',	2,	NULL,	NULL),
(88,	17,	'113', '113', 'Metals Processing',	2,	NULL,	NULL),
(89,	17,	'210', '210', 'Apparel',	2,	NULL,	NULL),
(90,	17,	'215', '215', 'Furniture, Furnishings and Interiors Manufacturing Industry',	2,	NULL,	NULL),
(91,	17,	'216', '216', 'Glass Industry Occupations',	2,	NULL,	NULL),
(92,	17,	'227', '227', 'Print and Printed Packaging',	2,	NULL,	NULL),
(93,	17,	'256', '256', 'Coatings Occupations',	2,	NULL,	NULL),
(94,	17,	'358', '358', 'Building Products Occupations',	2,	NULL,	NULL),
(95,	17,	'359', '359', 'Extractive and Mineral Processing Operations',	2,	NULL,	NULL),
(96,	17,	'234', '234', 'Marine Industry',	1,	'2012-04-24 00:00:00.000',	NULL),
(97,	13,	'209', '209', 'Cleaning and Support Services',	2,	NULL,	NULL),
(98,	13,	'213', '213', 'Property Services',	2,	NULL,	NULL),
(99,	20,	'221', '221', 'Housing',	2,	NULL,	NULL),
(100,	20,	'230', '230', 'Security',	2,	'2009-08-06 00:00:00.000',	NULL),
(101,	20,	'104', '104', 'Childrens Care, Learning and Development',	2,	NULL,	NULL),
(102,	20,	'214', '214', 'Emergency Fire Service Operations',	2,	NULL,	NULL),
(103,	20,	'236', '236', 'Health and Social Care',	2,	NULL,	NULL),
(104,	20,	'287', '287', 'Pharmacy Assistants and Technicians',	2,	NULL,	NULL),
(105,	20,	'290', '290', 'Community Justice',	2,	NULL,	NULL),
(106,	22,	'311', '311', 'Supporting Teaching and Learning in Schools',	2,	NULL,	NULL),
(107,	20,	'313', '313', 'Public Services',	2,	NULL,	NULL),
(108,	20,	'315', '315', 'Dental Nursing',	2,	NULL,	NULL),
(109,	20,	'319', '319', 'Support Services in Healthcare',	2,	NULL,	NULL),
(110,	20,	'340', '340', 'Youth Work',	2,	NULL,	NULL),
(111,	20,	'345', '345', 'Community Development',	2,	NULL,	NULL),
(112,	14,	'231', '231', 'Active Leisure and Learning',	2,	NULL,	NULL),
(113,	14,	'312', '312', 'Sporting Excellence',	2,	NULL,	NULL),
(114,	17,	'208', '208', 'Ceramics Manufacturing',	2,	NULL,	NULL),
(115,	13,	'323', '323', 'Storage and Warehousing',	2,	'2009-08-06 00:00:00.000',	NULL),
(116,	17,	'333', '333', 'Footwear and Leathergoods',	2,	NULL,	NULL),
(118,	1,	'366', '366', 'Marketing and Communications',	2,	NULL,	NULL),
(119,	17,	'369', '369', 'Specialised Process Operations (Nuclear)',	2,	NULL,	NULL),
(120,	7,	'370', '370', 'Surveying',	2,	NULL,	NULL),
(121,	3,	'368', '368', 'Photo Imaging For Staff Photographers',	2,	NULL,	NULL),
(122,	17,	'264', '264', 'Food and Drink Manufacturing	',2	,NULL,	NULL),
(123,	13,	'122', '122', 'Nail Services Apprentice',	2,	NULL,	NULL),
(124,	13,	'365', '365', 'Purchasing and  Supply Management',	1,	'2012-05-14 00:00:00.000',	NULL),
(125,	3,	'367', '367', 'Freelance Apprenticeship (Music Practitioner)',	2,	NULL,	NULL),
(126,	17,	'288', '288', 'Laboratory Technicians',	2,	NULL,	NULL),
(127,	13,	'372', '372', 'Logistics Operations Management',	2,	NULL,	NULL),
(128,	3,	'272', '272', 'Information and Library Services',	2,	NULL,	NULL),
(129,	20,	'374', '374', 'Security Systems',	2,	NULL,	NULL),
(130,	17,	'229', '229', 'Sea Fishing',	1,	'2012-04-24 00:00:00.000',	NULL),
(131,	2,	'375', '375', 'Horticulture',	2,	NULL,	NULL),
(132,	13,	'377', '377', 'Facilities Management',	2,	NULL,	NULL),
(133,	17,	'378', '378', 'Laboratory Technicians (SEMTA & COGENT)',	2,	NULL,	NULL),
(134,	3,	'379', '379', 'Creative & Digital Media',	2,	NULL,	NULL),
(135,	13,	'376', '376', 'Fashion & Textiles',	2,	NULL,	NULL),
(136,	20,	'373', '373', 'Providing Security Services',	2,	NULL,	NULL),
(137,	17,	'371', '371', 'Paper and Board Manufacturing',	2,	NULL,	NULL),
(139,	17,	'382', '382', 'Sustainable Resource Management',	2,	NULL,	NULL),
(140,	13,	'401', '401', 'Drinks Dispense Systems',	1,	NULL,	NULL),
(141,	20,	'406', '406', 'Courts, Tribunal and Prosecution Administration',	1,	NULL,	NULL),
(142,	20,	'407', '407', 'Policing',	1,	NULL,	NULL),
(143,	20,	'410', '410', 'Custodial Care',	1,	NULL,	NULL),
(144,	14,	'408', '408', 'Travel Services (People 1st)',	1,	NULL,	NULL),
(145,	20,	'409', '409', 'Emergency Fire Service Operations (Skills for Justice)',	1,	NULL,	NULL),
(146,	13,	'411', '411', 'Mail Services and Package Distribution (Skills for Logistics)',	1,	NULL,	NULL),
(147,	13,	'412', '412', 'Traffic Office  (Skills for Logistics)',	1,	NULL,	NULL),
(148,	13,	'413', '413', 'International Trade and Logistics',	1,	NULL,	NULL),
(149,	13,	'414', '414', 'Warehousing and Storage (Skills for Logistics)',	1,	NULL,	NULL),
(150,	13,	'415', '415', 'Commercial Moving', 	1,	NULL,	NULL),
(151,	13,	'402', '402', 'Hospitality and Catering (People 1st)',	2,	'2013-02-01 00:00:00.000',	NULL),
(152,	1,	'417', '417', 'Providing Financial Advice',	1,	NULL,	NULL),
(153,	15,	'418', '418', 'IT, Software, Web & Telecoms Professionals',	1,	NULL,	NULL),
(154,	15,	'419', '419', 'IT Application Specialist',	1,	NULL,	NULL),
(155,	17,	'403', '403', 'Food and Drink',	1,	NULL,	NULL),
(156,	22,	'420', '420', 'Supporting Teaching and Learning in Schools (LSIS)',	1,	NULL,	NULL),
(157,	20,	'421', '421', 'Security Systems (Skills for Security)',	1,	NULL,	NULL),
(158,	13,	'422', '422', 'Beauty Therapy (SkillsActive/Habia)',	1,	NULL,	NULL),
(159,	13,	'423', '423', 'Fashion and Textiles (Skillset)',	1,	NULL,	NULL),
(161,	2,	'438', '438', 'Game and Wildlife Management (Lantra)',	1,	NULL,	NULL),
(162,	2,	'439', '439', 'Animal Care (Lantra)',	1,	NULL,	NULL),
(163,	3,	'448', '448', 'Photo Imaging',	1,	NULL,	NULL),
(164,	3,	'449', '449', 'Creative and Digital Media (Skillset)',	1,	NULL,	NULL),
(165,	1,	'451', '451', 'Payroll (FSP)',	1,	NULL,	NULL),
(166,	1,	'452', '452', 'Bookkeeping',	1,	NULL,	NULL),
(167,	1,	'453', '453', 'Providing Mortgage Advice',	1,	NULL,	NULL),
(169,	1,	'454', '454', 'Accounting (FSP)',	1,	NULL,	NULL),
(170,	1,	'455', '455', 'Providing Financial Services (FSP)',	1,	NULL,	NULL),
(171,	17,	'433', '433', 'Vehicle Parts', 	1,	NULL,	NULL),
(172,	17,	'434', '434', 'Vehicle Body and Paint',	1,	NULL,	NULL),
(173,	17,	'436', '436', 'Vehicle Maintenance and Repair', 	1,	NULL,	NULL),
(174,	17,	'441', '441', 'Driving Goods Vehicles (Skills for Logistics)',	1,	NULL,	NULL),
(175,	17,	'427', '427', 'The Power Industry', 	1,	NULL,	NULL),
(176,	17,	'424', '424', 'Polymer Processing Operations (Cogent)',	1,	NULL,	NULL),
(177,	17,	'425', '425', 'Process Manufacturing',	1,	NULL,	NULL),
(178,	17,	'426', '426', 'Nuclear Working',	1,	NULL,	NULL),
(179,	17,	'428', '428', 'Rail Engineering (Track)',	1,	NULL,	NULL),
(180,	17,	'429', '429', 'Rail Services (Go Skills)',	1,	NULL,	NULL),
(181,	17,	'430', '430', 'Passenger Carrying Vehicle Driving (Bus and Coach)',	1,	NULL,	NULL),
(182,	17,	'431', '431', 'Bus and Coach Engineering and Maintenance',	1,	NULL,	NULL),
(183,	17,	'437', '437', 'Vehicle Fitting', 	1,	NULL,	NULL),
(184,	17,	'446', '446', 'The Gas Industry',	1,	NULL,	NULL),
(185,	17,	'469', '469', 'Ceramics Manufacturing (Proskills)',	1,	NULL,	NULL),
(186,	20,	'440', '440', 'Providing Security Services (Skills For Security)',	1,	NULL,	NULL),
(187,	20,	'445', '445', 'Children and Young People''s Workforce',	1,	NULL,	NULL),
(188,	20,	'450', '450', 'Libraries Records and IM Services',	1,	NULL,	NULL),
(189,	20,	'444', '444', 'Health and Social Care (Skills for Care)',	1,	NULL,	NULL),
(190,	20,	'447', '447', 'Youth Work (LSIS)',	1,	NULL,	NULL),
(191,	20,	'470', '470', 'Health - Pathology Support',	1,	NULL,	NULL),
(192,	20,	'471', '471', 'Health - Optical Retail',	1,	NULL,	NULL),
(193,	20,	'472', '472', 'Health - Blood Donor Support',	1,	NULL,	NULL),
(194,	20,	'473', '473', 'Health - Clinical Healthcare',	1,	NULL,	NULL),
(195,	20,	'474', '474', 'Health - Healthcare Support Services',	1,	NULL,	NULL),
(196,	20,	'475', '475', 'Health - Maternity and Paediatric Support',	1,	NULL,	NULL),
(197,	20,	'476', '476', 'Health - Emergency Care',	1,	NULL,	NULL),
(198,	20,	'477', '477', 'Health - Perioperative Support',	1,	NULL,	NULL),
(199,	20,	'478', '478', 'Health  - Allied Health Profession Support',	1,	NULL,	NULL),
(200,	20,	'479', '479', 'Health - Dental Nursing',	1,	NULL,	NULL),
(201,	20,	'480', '480', 'Health - Pharmacy Services',	1,	NULL,	NULL),
(202,	14,	'432', '432', 'Cabin Crew (Go Skills)',	1,	NULL,	NULL),
(203,	14,	'456', '456', 'Playwork',	1,	NULL,	NULL),
(204,	14,	'457', '457', 'Advanced Playwork',	1,	'2013-03-14 00:00:00.000',	NULL),
(205,	14,	'458', '458', 'Spectator Safety',	1,	NULL,	NULL),
(206,	14,	'459', '459', 'Advanced Spectator Safety',	1,	NULL,	NULL),
(207,	14,	'460', '460', 'Activity Leadership',	1,	NULL,	NULL),
(208,	14,	'461', '461', 'Advanced Fitness',	3,	'2012-07-17 00:00:00.000',	NULL),
(209,	14,	'462', '462', 'Exercise and Fitness',	1,	NULL,	NULL),
(210,	13,	'435', '435', 'Vehicle Sales', 	1,	NULL,	NULL),
(211,	13,	'443', '443', 'Retail (Skillsmart)',	1,	NULL,	NULL ),
(212,	13,	'442', '442', 'Logistics Operations',	1,	NULL,	NULL ),
(213,	17,	'416', '416', 'Signmaking (Cogent)',	1,	NULL,	NULL ),
(214,	1,	'485', '485', 'Sales and Telesales',	1,	NULL,	NULL ),
(215,	1,	'486', '486', 'Marketing',	1,	NULL,	NULL),
(216,	1,	'487', '487', 'Management',	1,	NULL,	NULL),
(217,	1,	'488', '488', 'Customer Service (CFA)',	1,	NULL,	NULL),
(218,	1,	'489', '489', 'Contact Centre Operations',	1,	NULL,	NULL),
(219,	1,	'490', '490', 'Business and Administration (CFA)',	1,	NULL,	NULL),
(220,	14,	'463', '463', 'Leisure Management',	1,	NULL,	NULL),
(221,	14,	'464', '464', 'Outdoor Programmes',	1,	NULL,	NULL),
(222,	14,	'465', '465', 'Sporting Excellence (SkillsActive)',	1,	NULL,	NULL),
(223,	14,	'466', '466', 'Leisure Operations',	1,	NULL,	NULL),
(224,	14,	'467', '467', 'Sports Development', 	1,	NULL,	NULL),
(225,	3,	'497', '497', 'Design (Creative and Cultural)',	1,	NULL,	NULL),
(226,	3,	'496', '496', 'Music Business',	1,	NULL,	NULL),
(227,	3,	'495', '495', 'Cultural and Heritage Venue operations',	1,	NULL,	NULL),
(228,	3,	'494', '494', 'Technical Theatre',	1,	NULL,	NULL),
(229,	3,	'493', '493', 'Costume and Wardrobe',	1,	NULL,	NULL),
(230,	3,	'492', '492', 'Community Arts',	1,	NULL,	NULL),
(231,	3,	'491', '491', 'Live Events and Promotion',	1,	NULL,	NULL),
(232,	13,	'498', '498', 'Cleaning and Environmental Services',	1,	NULL,	NULL),
(233,	20,	'499', '499', 'Housing (Asset Skills)',	1,	NULL,	NULL),
(234,	13,	'500', '500', 'Property Services (Asset Skills)',	1,	NULL,	NULL),
(235,	13,	'501', '501', 'Facilities Management (Asset Skills)',	1,	NULL,	NULL),
(236,	17,	'502', '502', 'Glass Industry',	1,	NULL,	NULL),
(237,	13,	'404', '404', 'Licensed Hospitality',	1,	NULL,	NULL),
(238,	17,	'503', '503', 'Production of Coatings',	1,	NULL,	NULL),
(239,	17,	'505', '505', 'Sustainable Resource Management (EU Skills)',	1,	NULL,	NULL),
(240,	17,	'506', '506', 'Laboratory and Science Technicians',	1,	NULL,	NULL ),
(241,	17,	'504', '504', 'Improving Operational Performance',	1,	NULL,	NULL ),
(242,	13,	'507', '507', 'Barbering (Skills Active/ Habia)',	1,	NULL,	NULL ),
(243,	13,	'508', '508', 'Hairdressing (Skills Active/ Habia)',	1,	NULL,	NULL),
(244,	13,	'509', '509', 'Nail Services (Skills Active/ Habia)',	1,	NULL,	NULL),
(245,	13,	'510', '510', 'Spa Therapy (Skills Active/ Habia)',	1,	NULL,	NULL),
(246,	17,	'405', '405', 'Aviation Operations on the Ground (Go Skills)',	1,	NULL,	NULL),
(247,	2,	'511', '511', 'Equine (Lantra)',	1,	NULL,	NULL),
(248,	17,	'517', '517', 'Advanced Engineering Construction',	1,	NULL,	NULL),
(249,	17,	'518', '518', 'Engineering Construction (ECITB)',	2,	'2012-05-28 00:00:00.000',	NULL),
(250,	7,	'519', '519', 'Construction Specialist',	1,	NULL,	NULL),
(251,	7,	'520', '520', 'Construction Civil Engineering', 	1,	NULL,	NULL),
(252,	7,	'521', '521', 'Construction Technical', 	1,	NULL,	NULL),
(253,	7,	'522', '522', 'Construction Building', 	1,	NULL,	NULL),
(254,	7,	'512', '512', 'Plumbing and Heating',	1,	NULL,	NULL),
(255,	17,	'513', '513', 'Electrotechnical (Summit Skills)',	1,	NULL,	NULL),
(256,	7,	'514', '514', 'Refrigeration and Air Conditioning',	1,	NULL,	NULL),
(257,	7,	'515', '515', 'Heating and Ventilation',	1,	NULL,	NULL),
(258,	7,	'516', '516', 'Domestic Heating',	1,	NULL,	NULL),
(259,	2,	'523', '523', 'Floristry (Lantra)',	1,	NULL,	NULL),
(260,	2,	'524', '524', 'Environmental Conservation (Lantra)',	1,	NULL,	NULL),
(261,	2,	'525', '525', 'Landbased Engineering',	1,	NULL,	NULL),
(262,	2,	'526', '526', 'Farriery (Lantra)',	1,	NULL,	NULL),
(263,	2,	'527', '527', 'Horticulture (Lantra)',	1,	NULL,	NULL),
(264,	2,	'528', '528', 'Agriculture (Lantra)',	1,	NULL,	NULL),
(265,	2,	'529', '529', 'Veterinary Nursing (Lantra)',	1,	NULL,	NULL),
(266,	2,	'530', '530', 'Fencing (Lantra)',	1,	NULL,	NULL),
(267,	2,	'531', '531', 'Trees and Timber (Lantra)',	1,	NULL,	NULL),
(268,	20,	'536', '536', 'Employment Related Services',	1,	NULL,	NULL),
(269,	20,	'537', '537', 'HM Forces',	1,	NULL,	NULL),
(270,	20,	'538', '538', 'Witness Care',	1,	NULL,	NULL),
(271,	7,	'532', '532', 'Surveying (Asset Skills)',	1,	NULL,	NULL),
(272,	17,	'539', '539', 'Engineering Manufacture (Operator & Semi Skilled)',	1,	NULL,	NULL),
(273,	17,	'540', '540', 'Engineering Manufacture (Craft and Technician)',	1,	NULL,	NULL),
(274,	17,	'535', '535', 'The Water Industry',	1,	NULL,	NULL),
(275,	17,	'542', '542', 'Print and Printed Packaging (Proskills)',	1,	NULL,	NULL),
(276,	22,	'541', '541', 'Learning and Development (LSIS)',	1,	NULL,	NULL),
(277,	7,	'543', '543', 'Building Services Engineering Technology', 	1,	NULL,	NULL),
(278,	17,	'544', '544', 'Rail Traction and Rolling Stock Engineering',	1,	NULL,	NULL),
(279,	17,	'545', '545', 'Rail Infrastructure Engineering',	1,	NULL,	NULL),
(280,	17,	'546', '546', 'Combined Manufacturing Processes', 	1,	NULL,	NULL),
(281,	17,	'548', '548', 'Jewellery, Silversmithing and Allied Trades',	1,	NULL,	NULL),
(282,	17,	'550', '550', 'Advanced Manufacturing Engineering',	1,	NULL,	NULL),
(283,	20,	'549', '549', 'Local Taxation and Benefits',	1,	NULL,	NULL),
(284,	17,	'551', '551', 'Furniture, Furnishing and Interiors', 	1,	NULL,	NULL),
(285,	1,	'552', '552', 'Enterprise',	1,	NULL,	NULL),
(286,	17,	'553', '553', 'Extractives',	1,	NULL,	NULL),
(287,	14,	'561', '561', 'Coaching',	1,	NULL,	NULL),
(288,	17,	'562', '562', 'Operations and Quality Improvement',	1,	NULL,	NULL),
(289,	17,	'560', '560', 'Maritime Occupations',	1,	NULL,	NULL),
(291,	24,	'563', '563', 'Life Sciences',	1,	NULL,	NULL),
(292,	1,	'558', '558', 'Volunteer Management',	1,	NULL,	NULL),
(293,	13,	'566', '566', 'Supply Chain Management',	1,	NULL,	NULL),
(294,	1,	'556', '556', 'Campaigning',	1,	NULL,	NULL),
(295,	1,	'557', '557', 'Fundraising',	1,	NULL,	NULL),
(296,	1,	'565', '565', 'Legal Services',	1,	NULL,	NULL),
(297,	17,	'564', '564', 'Engineering Construction – ECITB',	1,	NULL,	NULL),
(298,	20,	'567', '567', 'Health (Informatics)', 	1,	NULL,	NULL),
(299,	2,	'568', '568', 'Fish Husbandry and Fisheries Management',	1,	NULL,	NULL),
(300,	1,	'572', '572', 'Public Relations',	1,	NULL,	NULL),
(301,	3,	'555', '555', 'SetCrafts',	1,	NULL,	NULL),
(302,	7,	'554', '554', 'Building Energy Management Systems',	1,	NULL,	NULL),
(303,	1,	'575', '575', 'Professional Services',	1,	NULL,	NULL),
(304,	2,	'569', '569', 'Nursing Assistants in a Veterinary Environment',	1,	NULL,	NULL),
(305,	17,	'570', '570', 'Smart Meter Installations (Dual Fuel)',	1,	NULL,	NULL),
(306,	20,	'571', '571', 'Locksmithing',	1,	NULL,	NULL),
(307,	17,	'576', '576', 'Wood & Timber Processing and Merchants Industry',	1,	NULL,	NULL),
(308,	1,	'574', '574', 'Human Resource Management',	1,	NULL,	NULL),
(309,	13,	'578', '578', 'Funeral Operations and Services',	1,	NULL,	NULL),
(310,	13,	'577', '577', 'Express Logistics',	1,	NULL,	NULL),
(311,	13,	'580', '580', 'Hospitality Management',	1,	NULL,	NULL),
(312,	1,	'579', '579', 'Social Media and Digital Marketing',	1,	NULL,	NULL),
(313,	1,	'581', '581', 'Business Innovation and Growth',	1,	NULL,	NULL),
(314,	13,	'582', '582', 'Catering and Professional Chefs',	1,	NULL,	NULL),
(315,	13,	'583', '583', 'Hospitality',	1,	NULL,	NULL),
(316,	20,	'584', '584', 'Care Leadership and Management',	1,	NULL,	NULL),
(317,	17,	'585', '585', 'Fashion and Textiles:Technical',	1,	NULL,	NULL),
(318,	3,	'586', '586', 'Advertising & Marketing Communications', 	1,	NULL,	NULL),
(319,	13,	'588', '588', 'Energy Assessment and Advice',	1,	NULL,	NULL),
(320,	1,	'573', '573', 'Project Management',	1,	NULL,	NULL),
(321,	17,	'559', '559', 'Building Products Industry Occupations',	1,	NULL,	NULL),
(322,	22,	'592', '592', 'Learning Support',	1,	NULL,	NULL),
(323,	17,	'593', '593', 'Mineral Products Technology',	1,	NULL,	NULL),
(324,	20,	'591', '591', 'Professional Development for Work Based Learning Practitioners', 	1,	NULL,	NULL),
(325,	1,	'590', '590', 'Banking',	1,	NULL,	NULL),
(326,	1,	'589', '589', 'Insurance',	1,	NULL,	NULL),
(327,	3,	'599', '599', 'Journalism',	1,	NULL,	NULL),
(328,	1,	'594', '594', 'Recruitment',	1,	NULL,	NULL),
(329,	17,	'600', '600', 'Power Engineering (EU Skills)',	1,	NULL,	NULL),
(330,	17,	'597', '597', 'Metal Processing and Allied Operations (SEMTA)',	1,	NULL,	NULL),
(331,	17,	'596', '596', 'Composite Engineering (SEMTA)',	1,	NULL,	NULL),
(332,	20,	'602', '602', 'Health Assistant Practitioner - (Skills for Health)',	1,	NULL,	NULL),
(333,	20,	'603', '603', 'Intelligence Analysis (Skills for Justice)',	1,	NULL,	NULL),
(334,	3,	'605', '605', 'Broadcasting Technology',	1,	NULL,	NULL),
(335,	17,	'587', '587', 'Consumer Electrical and Electronic Products',	1,	NULL,	NULL),
(336,	2,	'598', '598', 'Animal Technology (Lantra)',	1,	NULL,	NULL),
(337,	1,	'601', '601', 'Legal Advice',	1,	NULL,	NULL),
(338,	17,	'604', '604', 'Engineering Environmental Technologies',	1,	NULL,	NULL),
(999,	99,	'999', '999', 'Traineeship',	1,	NULL,	NULL),
(1000,	3,	'609', '609', 'Sound Recording, Engineering and Studio Facilities',	1,	NULL,	NULL),
(1001,	17,	'607', '607', 'Explosives, Storage & Maintenance',	1,	NULL,	NULL),
(1002,	13,	'608', '608', 'Retail Management  (Higher)',	1,	NULL,	NULL),
(1003,	7,	'612', '612', 'Construction Management',	1,	NULL,	NULL),
(1004,	17,	'606', '606', 'Multi-Skilled Vehicle Collision Repair',	1,	NULL,	NULL),
(1005,	3,	'610', '610', 'Broadcast Production', 	1,	NULL,	NULL),
(1006,	22,	'613', '613', 'Supporting Teaching and Learning in Physical Education', 	1,	NULL,	NULL),
(1007,	17,	'615', '615', 'Professional Aviation Pilot Practice', 	1,	NULL,	NULL),
(1009,	1,	'616', '616', 'Trade Business',	1,	NULL,	NULL),
(1010,	1,	'614', '614', 'Criminal Investigation',	1,	NULL,	NULL),
(1011,	15,	'611', '611', 'Information Security',	1,	NULL,	NULL),
(1012,	3,	'622', '622', 'Interactive Design and Development',	1,	NULL,	NULL),
(1013,	3,	'621', '621', 'Craft and Technical Roles in Film and Television',	1,	NULL,	NULL),
(1014,	17,	'623', '623', 'Rail Engineering Overhead Line Construction',	1,	NULL,	NULL),
(1015,	17,	'625', '625', 'Blacksmithing',	1,	NULL,	NULL),
(1016,	17,	'626', '626', 'Automotive Management and Leadership',	1,	NULL,	NULL),
(1017,	17,	'627', '627', 'Advanced Diagnostics and Management Principles', 	1,	NULL,	NULL),
(1018,	13,	'628', '628', 'International Supply Chain Management',	1,	NULL,	NULL),
(1019,	3,	'636', '636', 'Digital Learning Design',	1,	NULL,	NULL),
(1020,	17,	'639', '639', 'Automotive Clay Modelling',	1,	NULL,	NULL),
(1021,	100,	'00', '00', 'Test - DO NOT USE',	2,	NULL,	NULL),
(1023,	100,	'001', '001', 'Public Service - Operational delivery officer (Level 3)',	1,	NULL,	NULL),
(1024,	100,	'002', '002', 'Cyber Security',	1,	NULL,	NULL),
(1025,	100,	'003', '003', 'Public Sector Commercial',	1,	NULL,	NULL),
(1026,	13,		'638', '638', 'Procurement',	1,	NULL,	NULL),
(1027,	100,	'004', '004', 'Aerospace manufacturing fitter (Level 3)',	1,	NULL,	NULL),
(1028,	100,	'005', '005', 'Mechatronics maintenance technician (Level 3)',	1,	NULL,	NULL),
(1029,	100,	'006', '006', 'Control/technical support engineer (Level 6)',	1,	NULL,	NULL),
(1030,	100,	'007', '007', 'Electrical/electronic technical support engineer (Level 6)',	1,	NULL,	NULL),
(1031,	100,	'008', '008', 'Manufacturing engineer (Level 6)',	1,	NULL,	NULL),
(1032,	100,	'009', '009', 'Product design and development engineer (Level 6)',	1,	NULL,	NULL),
(1033,	100,	'010', '010', 'Product design and development technician (Level 3)',	1,	NULL,	NULL),
(1034,	100,	'011', '011', 'Network engineer (Level 4)',	1,	NULL,	NULL),
(1035,	100,	'012', '012', 'Software developer (Level 4)',	1,	NULL,	NULL),
(1036,	100,	'013', '013', 'Power network craftsperson (Level 3)',	1,	NULL,	NULL),
(1037,	100,	'014', '014', 'Relationship manager - banking (Level 6)',	1,	NULL,	NULL),
(1038,	100,	'015', '015', 'Financial services administrator (Level 3)',	1,	NULL,	NULL),
(1039,	100,	'016', '016', 'Food and drink maintenance engineer (Level 3)',	1,	NULL,	NULL),
(1040,	100,	'017', '017', 'Laboratory technician (Level 3)',	1,	NULL,	NULL),
(1041,	100,	'018', '018', 'Science manufacturing technician (Level 3)',	1,	NULL,	NULL),
(1042,	100,	'019', '019', 'Acturial technician (Level 4)',	1,	NULL,	NULL),
(1043,	100,	'020', '020', 'Dental technician (Level 5)',	1,	NULL,	NULL),
(1044,	100,	'021', '021', 'Dental laboratory assistant (Level 3)',	1,	NULL,	NULL),
(1045,	100,	'022', '022', 'Dental practice manager (Level 4)',	1,	NULL,	NULL),
(1046,	100,	'023', '023', 'Degree Apprenticeship Technology Solutions',	1,	NULL,	NULL),
(1047,	100,	'024', '024', 'Golf greenkeeper (Level 2)',	1,	NULL,	NULL),
(1048,	100,	'025', '025', 'Junior journalist (Level 3)',	1,	NULL,	NULL),
(1049,	100,	'026', '026', 'Property maintenance operative (Level 2)',	1,	NULL,	NULL),
(1050,	100,	'027', '027', 'Railway engineering design technician (Level 3)',	1,	NULL,	NULL),
(1051,	3,		'624', '624', 'Creative Craft Practitioner',	1,	NULL,	NULL),
(1052,	1,		'620', '620', 'Business and Professional Administration',	1,	NULL,	NULL),
(1053,	100,	'028', '028', 'Digital & technology solutions professional (Level 6)',	1,	NULL,	NULL),
(1054,	100,	'029', '029', 'Financial services customer advisor (Level 2)',	1,	NULL,	NULL),
(1055,	100,	'030', '030', 'Investment operations administrator (Level 2)',	1,	NULL,	NULL),
(1056,	100,	'031', '031', 'Investment operations technician (Level 3)',	1,	NULL,	NULL),
(1057,	100,	'032', '032', 'Investment operations specialist (Level 4)',	1,	NULL,	NULL),
(1058,	100,	'033', '033', 'Senior financial services customer advisor (Level 3)',	1,	NULL,	NULL),
(1059,	100,	'034', '034', 'Workplace Pensions (Administrator or Consultant)',	1,	NULL,	NULL),
(1060,	100,	'035', '035', 'Able seafarer- deck (Level 2)',	1,	NULL,	NULL),
(1061,	100,	'036', '036', 'Nuclear welding inspection technician (Level 4)',	1,	NULL,	NULL),
(1062,	100,	'037', '037', 'Aerospace engineer (Level 6)',	1,	NULL,	NULL),
(1063,	100,	'038', '038', 'Aerospace software development engineer (Level 6)',	1,	NULL,	NULL),
(1064,	100,	'039', '039', 'Conveyancing technician (Level 4)',	1,	NULL,	NULL),
(1065,	100,	'040', '040', 'Licensed conveyancer (Level 4)',	1,	NULL,	NULL),
(1066,	100,	'041', '041', 'Systems engineering masters level (Level 7)',	1,	NULL,	NULL),
(1067,	100,	'042', '042', 'Installation electrician/maintenance electrician (Level 3)',	1,	NULL,	NULL),
(1068,	100,	'043', '043', 'Dual fuel smart meter installer (Level 2)',	1,	NULL,	NULL),
(1069,	100,	'044', '044', 'Water process technician (Level 3)',	1,	NULL,	NULL),
(1070,	100,	'045', '045', 'Paraplanner (Level 4)',	1,	NULL,	NULL),
(1071,	100,	'046', '046', 'Chartered legal executive (Level 6)',	1,	NULL,	NULL),
(1072,	100,	'047', '047', 'Paralegal (Level 3)',	1,	NULL,	NULL),
(1073,	100,	'048', '048', 'Solicitor (Level 7)',	1,	NULL,	NULL),
(1074,	100,	'049', '049', 'Laboratory Scientist (Level 5)',	1,	NULL,	NULL),
(1075,	100,	'050', '050', 'Science industry maintenance technician (Level 3)',	1,	NULL,	NULL),
(1076,	100,	'051', '051', 'Nuclear health physics monitor (Level 2)',	1,	NULL,	NULL),
(1077,	100,	'052', '052', 'Nuclear scientist and nuclear engineer (Level 6)',	1,	NULL,	NULL),
(1078,	100,	'053', '053', 'Refrigeration air conditioning & heat pump engineering technician (Level 3)',	1,	NULL,	NULL),
(1079,	100,	'054', '054', 'Chartered surveyor (Level 6)',	1,	NULL,	NULL),
(1080,	100,	'055', '055', 'Surveying technician (Level 3)',	1,	NULL,	NULL),
(1081,	100,	'056', '056', 'Butcher (Level 2)',	1,	NULL,	NULL),
(1082,	100,	'057', '057', 'Utilities engineering technician (Level 3)',	1,	NULL,	NULL),
(1083,	100,	'058', '058', 'Chartered manager degree apprenticeship (Level 6)',	1,	NULL,	NULL),
(1085,	100,	'059', '059', 'Infrastructure Technician (Level 3)',	1,	NULL,	NULL)
) 

AS Source (ApprenticeshipFrameworkId, ApprenticeshipOccupationId, CodeName, ShortName, FullName, ApprenticeshipFrameworkStatusTypeId, ClosedDate, PreviousApprenticeshipOccupationId) 
ON Target.ApprenticeshipFrameworkId = Source.ApprenticeshipFrameworkId 
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET ApprenticeshipOccupationId = Source.ApprenticeshipOccupationId, CodeName = Source.CodeName, ShortName = Source.ShortName, FullName = Source.FullName, ApprenticeshipFrameworkStatusTypeId = Source.ApprenticeshipFrameworkStatusTypeId, ClosedDate = Source.ClosedDate, PreviousApprenticeshipOccupationId = Source.PreviousApprenticeshipOccupationId
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (ApprenticeshipFrameworkId, ApprenticeshipOccupationId, CodeName, ShortName, FullName, ApprenticeshipFrameworkStatusTypeId, ClosedDate, PreviousApprenticeshipOccupationId) 
VALUES (ApprenticeshipFrameworkId, ApprenticeshipOccupationId, CodeName, ShortName, FullName, ApprenticeshipFrameworkStatusTypeId, ClosedDate, PreviousApprenticeshipOccupationId) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[ApprenticeshipFramework] OFF
GO