QUEST_SCE_PLOTBEGIN = {
	title = 'IDS_PROPQUEST_SCENARIO_INC_000327',
	character = 'MaFl_Martinyc',
	end_character = 'MaSa_Gothante',
	start_requirements = {
		min_level = 20,
		max_level = 129,
		job = { 'JOB_VAGRANT', 'JOB_MERCENARY', 'JOB_ACROBAT', 'JOB_ASSIST', 'JOB_MAGICIAN', 'JOB_KNIGHT', 'JOB_BLADE', 'JOB_JESTER', 'JOB_RANGER', 'JOB_RINGMASTER', 'JOB_BILLPOSTER', 'JOB_PSYCHIKEEPER', 'JOB_ELEMENTOR', 'JOB_KNIGHT_MASTER', 'JOB_BLADE_MASTER', 'JOB_JESTER_MASTER', 'JOB_RANGER_MASTER', 'JOB_RINGMASTER_MASTER', 'JOB_BILLPOSTER_MASTER', 'JOB_PSYCHIKEEPER_MASTER', 'JOB_ELEMENTOR_MASTER', 'JOB_KNIGHT_HERO', 'JOB_BLADE_HERO', 'JOB_JESTER_HERO', 'JOB_RANGER_HERO', 'JOB_RINGMASTER_HERO', 'JOB_BILLPOSTER_HERO', 'JOB_PSYCHIKEEPER_HERO', 'JOB_ELEMENTOR_HERO' },
		previous_quest = 'QUEST_SCE_REASONCONDIV',
	},
	rewards = {
		gold = 12500,
		exp = 1740,
		items = {
			{ id = 'II_SYS_SYS_QUE_GOTNOMINATE', quantity = 1, sex = 'Any' },
		},
	},
	end_conditions = {
	},
	dialogs = {
		begin = {
			'IDS_PROPQUEST_SCENARIO_INC_000328',
			'IDS_PROPQUEST_SCENARIO_INC_000329',
		},
		begin_yes = {
			'IDS_PROPQUEST_SCENARIO_INC_000330',
		},
		begin_no = {
			'IDS_PROPQUEST_SCENARIO_INC_000331',
		},
		completed = {
			'IDS_PROPQUEST_SCENARIO_INC_000332',
			'IDS_PROPQUEST_SCENARIO_INC_000333',
			'IDS_PROPQUEST_SCENARIO_INC_000334',
		},
		not_finished = {
			'IDS_PROPQUEST_SCENARIO_INC_000335',
		},
	}
}
